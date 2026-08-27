using MySql.Data.MySqlClient;
using StorageI.ModelsStroevkaMySql;
using System;
using System.Collections.Generic;

namespace stroevkaI.Services
{
    public class DataLoader
    {


        public DataLoader()
        {

        }

        public void getRawData() { 
        } 



        public List<Pch> LoadPchs()
        {
            // Выгружаем все ПЧ
            return FireEquipsPivotRepository.getPchList();
        }

        public List<Psg> LoadPsgs()
        {
            return FireEquipsPivotRepository.getPsgList();
        }

        public List<Sredstva> LoadSredstva(DateTime? date = null)
        {
            // Загружаем все записи, либо фильтруем по дате (пока без фильтра)
            return FireEquipsPivotRepository.GetAllSredstva();
        }

        public List<Sostav> LoadSostav(DateTime? date = null)
        {
            return FireEquipsPivotRepository.GetAllSostav();
        }

        public List<Sizod> LoadSizod(DateTime? date = null)
        {
            return FireEquipsPivotRepository.GetAllSizod();
        }

        public List<Pena> LoadPenas(DateTime? date = null)
        {
            return FireEquipsPivotRepository.GetAllPenas();
        }

        public List<Kostym> LoadKostyms(DateTime? date = null)
        {
            return FireEquipsPivotRepository.GetAllKostyms();
        }
    }
}