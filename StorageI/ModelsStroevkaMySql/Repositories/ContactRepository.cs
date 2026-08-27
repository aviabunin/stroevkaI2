// StorageI/ModelsStroevkaMySql/Repositories/ContactRepository.cs
using System;
using System.Collections.Generic;
using System.Linq;
using StorageI.ModelsStroevkaMySql;

namespace StorageI.ModelsStroevkaMySql.Repositories
{
    public class ContactRepository
    {
        private readonly stroevkaContext _context;

        public ContactRepository()
        {
            _context = new stroevkaContext();
        }

        #region Contacts

        public List<Contact> LoadContacts(int pchId, int karaul)
        {
            return _context.Contacts
                .Where(c => c.SubdivisionId == pchId && c.Karaul == karaul)
                .OrderBy(c => c.Norder)
                .ToList();
        }

        public bool SaveContact(Contact contact)
        {
            try
            {
                if (contact.Id == 0)
                    _context.Contacts.Add(contact);
                else
                {
                    var existing = _context.Contacts.Find(contact.Id);
                    if (existing != null)
                        _context.Entry(existing).CurrentValues.SetValues(contact);
                }
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool SaveContacts(List<Contact> contacts)
        {
            try
            {
                foreach (var contact in contacts)
                {
                    if (contact.Id == 0)
                        _context.Contacts.Add(contact);
                    else
                    {
                        var existing = _context.Contacts.Find(contact.Id);
                        if (existing != null)
                            _context.Entry(existing).CurrentValues.SetValues(contact);
                    }
                }
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteContact(int id)
        {
            try
            {
                var contact = _context.Contacts.Find(id);
                if (contact != null)
                {
                    _context.Contacts.Remove(contact);
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public bool CreateContactsForAllKarauls(int pchId, int postId, string postName,
            int subdivisionId, string subdivisionName, string garnizonName)
        {
            try
            {
                var post = _context.Posts.Find(postId);
                int norder = post?.Norder ?? 1;

                for (int karaul = 1; karaul <= 4; karaul++)
                {
                    var exists = _context.Contacts
                        .Any(c => c.SubdivisionId == subdivisionId && c.Karaul == karaul && c.Post == postName);

                    if (!exists)
                    {
                        var contact = new Contact
                        {
                            GarnizonId = 0,
                            Karaul = karaul,
                            PostId = postId,
                            Post = postName,
                            SubdivisionId = subdivisionId,
                            Subdivision = subdivisionName,
                            NameGarnizone = garnizonName,
                            Mdate = DateTime.Now.Date,
                            EditTime = DateTime.Now,
                            Norder = norder,
                            Fio = "",
                            TfMobil = "",
                            TfWork = "",
                            TfDom = "",
                            Posyvnoy = "",
                            Excel = ""
                        };

                        _context.Contacts.Add(contact);
                    }
                }

                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region Posts

        public List<Post> LoadPosts()
        {
            return _context.Posts
                .OrderBy(p => p.Norder)
                .ToList();
        }

        public Post GetPostById(int id)
        {
            return _context.Posts.Find(id);
        }

        #endregion

        #region Personals

        /// <summary>
        /// Получение списка сотрудников ПЧ с их разрешёнными должностями
        /// Исключаем ошибочные записи где psg_id = subdivision_id
        /// </summary>
        public List<PersonalWithPosts> LoadPersonalsWithPosts(int pchId)
        {
            var query = from p in _context.Personals
                        where p.SubdivisionId == pchId
                           && p.Inwork == 1
                           && p.PsgId != p.SubdivisionId  // Исключаем ошибочные записи
                        select new PersonalWithPosts
                        {
                            Personal = p,
                            AllowedPosts = (from pp in _context.Personalposts
                                            join post in _context.Posts on pp.PostId equals post.Id
                                            where pp.PersonalId == p.Id
                                            select post).ToList()
                        };

            return query
                .Where(x => x.AllowedPosts.Any())
                .OrderBy(x => x.Personal.F)
                .ToList();
        }

        /// <summary>
        /// Получение сотрудников по должности (только те, у кого разрешена эта должность)
        /// Исключаем ошибочные записи где psg_id = subdivision_id
        /// </summary>
        public List<PersonalWithPosts> LoadPersonalsByPost(int pchId, string postName)
        {
            var query = from p in _context.Personals
                        join pp in _context.Personalposts on p.Id equals pp.PersonalId
                        join post in _context.Posts on pp.PostId equals post.Id
                        where p.SubdivisionId == pchId
                           && p.Inwork == 1
                           && p.PsgId != p.SubdivisionId  // Исключаем ошибочные записи
                           && post.Name == postName
                        select new PersonalWithPosts
                        {
                            Personal = p,
                            AllowedPosts = new List<Post> { post }
                        };

            return query
                .OrderBy(x => x.Personal.F)
                .ToList();
        }

        /// <summary>
        /// Получение всех сотрудников ПЧ (для редактирования в отдельной вкладке)
        /// Исключаем ошибочные записи где psg_id = subdivision_id
        /// </summary>
        public List<Personal> LoadAllPersonals(int pchId)
        {
            return _context.Personals
                .Where(p => p.SubdivisionId == pchId
                    && p.Inwork == 1
                    && p.PsgId != p.SubdivisionId)  // Исключаем ошибочные записи
                .OrderBy(p => p.F)
                .ToList();
        }

        /// <summary>
        /// Сохранение сотрудника
        /// </summary>
        public bool SavePersonal(Personal personal)
        {
            try
            {
                if (personal.Id == 0)
                    _context.Personals.Add(personal);
                else
                {
                    var existing = _context.Personals.Find(personal.Id);
                    if (existing != null)
                        _context.Entry(existing).CurrentValues.SetValues(personal);
                }
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Сохранение разрешённых должностей для сотрудника
        /// </summary>
        public bool SavePersonalPosts(int personalId, List<int> postIds)
        {
            try
            {
                var oldPosts = _context.Personalposts
                    .Where(pp => pp.PersonalId == personalId);
                _context.Personalposts.RemoveRange(oldPosts);

                foreach (var postId in postIds)
                {
                    _context.Personalposts.Add(new Personalpost
                    {
                        PersonalId = personalId,
                        PostId = postId
                    });
                }

                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Удаление сотрудника (мягкое удаление - помечаем Inwork = 0)
        /// </summary>
        public bool DeletePersonal(int personalId)
        {
            try
            {
                var personal = _context.Personals.Find(personalId);
                if (personal != null)
                {
                    personal.Inwork = 0;
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region Вспомогательные классы

        public class PersonalWithPosts
        {
            public Personal Personal { get; set; }
            public List<Post> AllowedPosts { get; set; }
            public string FullName => $"{Personal.F} {Personal.I} {Personal.O}".Trim();
            public string AllowedPostsList => string.Join(", ", AllowedPosts.Select(p => p.Name));
        }

        #endregion
    }
}