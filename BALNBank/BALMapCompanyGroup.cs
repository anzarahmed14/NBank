using BOLNBank;
using DALNBank;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BALNBank
{
    public class BALMapCompanyGroup
    {
        List<clsMapCompanyGroup> list;
        List<SqlParameter> plist;
        clsMapCompanyGroup obj;

        #region CREATE

        public string CreateMapCompanyGroup(List<long> companyIds, long companyGroupId)
        {
            DataTable dtCompanyIds = CreateCompanyIdTable(companyIds);

            string message = (new DALMapCompanyGroup())
                .CreateMapCompanyGroup(
                    "CreateMapCompanyGroup",
                    dtCompanyIds,
                    companyGroupId);

            return message;
        }

        #endregion

        #region UPDATE

        public string UpdateMapCompanyGroup(List<long> companyGroupIds, long newCompanyId)
        {
            DataTable dtGroupIds = CreateCompanyIdTable(companyGroupIds);

            string message = (new DALMapCompanyGroup())
                .UpdateMapCompanyGroup(
                    "UpdateMapCompanyGroup",
                    dtGroupIds,
                    newCompanyId);

            return message;
        }

        #endregion

        #region GET LIST

        public List<clsMapCompanyGroup> GetMapCompanyGroupList(long companyId)
        {
            plist = new List<SqlParameter>();
            plist.Add(new SqlParameter("@CompanyId", SqlDbType.BigInt) { Value = companyId });

            list = (new DALMapCompanyGroup())
                .GetMapCompanyGroupList("GetMapCompanyGroup", plist);

            return list;
        }

        #endregion

        #region HELPER

        private DataTable CreateCompanyIdTable(List<long> groupIds)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("CompanyId", typeof(long));

            foreach (var id in groupIds)
                dt.Rows.Add(id);

            return dt;
        }

        #endregion

        public List<clsCompanyGroupList> GetMapCompanyGroupList(string CompanyGroupName = ""
    )
        {
            List<SqlParameter> plist = new List<SqlParameter>();

          
            plist.Add(new SqlParameter("@CompanyGroupName", SqlDbType.NVarChar, 100)
            { Value = CompanyGroupName });

            return new DALMapCompanyGroup()
                .GetCompanyGroupSummary("GetCompanyGroupMappingList", plist);
        }
        public List<clsMapCompanyGroup> GetByCompanyGroupId(long companyGroupId)
        {
            plist = new List<SqlParameter>();
            plist.Add(new SqlParameter("@CompanyGroupId", SqlDbType.BigInt)
            {
                Value = companyGroupId
            });

            list = (new DALMapCompanyGroup())
                .GetByCompanyGroupId(
                    "GetMapCompanyGroupByCompanyGroupId",
                    plist);

            return list;
        }
    }

}
