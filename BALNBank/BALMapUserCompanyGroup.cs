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
    public class BALMapUserCompanyGroup
    {
        List<clsUserCompanyGroupMapping> list;
        List<SqlParameter> plist;

        public string Create(long userId, List<long> companyGroupIds)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("CompanyGroupId", typeof(long));

            foreach (var id in companyGroupIds)
                dt.Rows.Add(id);

            return new DALMapUserCompanyGroup()
                .Create("CreateMapUserCompanyGroup", userId, dt);
        }

        public List<clsMapUserCompanyGroup> GetByUserId(long userId)
        {
            return new DALMapUserCompanyGroup()
                .GetByUserId("GetUserCompanyGroupByUserId", userId);
        }

        public List<clsUserCompanyGroupMapping> GetList(
        string companyGroupName = "")
        {
            plist = new List<SqlParameter>();

            plist.Add(
                new SqlParameter("@CompanyGroupName", SqlDbType.NVarChar, 100)
                {
                    Value = companyGroupName
                });

            list = (new DALMapUserCompanyGroup())
                .GetList("GetUserCompanyGroupMappingList", plist);

            return list;
        }
        public string Update(long userId, List<long> companyGroupIds)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("CompanyGroupId", typeof(long));

            foreach (var id in companyGroupIds)
                dt.Rows.Add(id);

            return new DALMapUserCompanyGroup()
                .Update("UpdateMapUserCompanyGroup", userId, dt);
        }

    }
}
