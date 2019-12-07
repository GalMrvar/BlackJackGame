using BlackjackDBLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameCardLib
{
    public class ResultsController
    {
        #region Fields

        private DatabaseController databaseController = new DatabaseController();

        #endregion

        #region Methods

        public List<ResultData> GetResultData()
        {
            List <ResultData> resultData = databaseController.GetLast20results();
            return resultData;
        }

        /// <summary>
        /// deleting player on Id
        /// </summary>
        /// <param name="data"></param>
        public void DeletePlayer(ResultData data)
        {
            databaseController.DeletePlayer(data);
        }

        /// <summary>
        /// updating player on Id
        /// </summary>
        /// <param name="data"></param>
        public void UpdatePlayer(ResultData data)
        {
            databaseController.UpdatePlayer(data);
        }

        public List<ResultData> SearchByString(string str)
        {
            return databaseController.SearchByString( str);
        }
        #endregion

    }
}
