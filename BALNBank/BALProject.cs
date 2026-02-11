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
    public class BALProject
    {
        List<clsProject> list;
        List<SqlParameter> plist;
        clsProject obj;
        Dictionary<string, long> projects;
        public List<clsProject> GetProjectList(string ProjectName = "")
        {
            plist = new List<SqlParameter>();
            plist.Add(new SqlParameter("@ProjectName", SqlDbType.NVarChar, 100) { Value = ProjectName });

            list = (new DALProject().GetProjectList("GetProject", plist));
            return list;
        }
        public clsProject GetProject(long ProjectID)
        {
            plist = new List<SqlParameter>();
            plist.Add(new SqlParameter("@ProjectID", SqlDbType.BigInt) { Value = ProjectID });
            obj = (new DALProject().GetProject("GetProject", plist));
            return obj;
        }
        public Dictionary<string, long> LoadProjects(long CompanyID)
        {
            projects = (new DALProject().LoadProjects(CompanyID));
            return projects;
        }
    }
}
