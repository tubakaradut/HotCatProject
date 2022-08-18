using HC.Dal.Context;

namespace HC.BLL.SingletonPattern
{
    public class DbTool
    {
        DbTool() { }

        static HCContext _dbInstance;

        public static HCContext DBInstance
        {
            get
            {

                if (_dbInstance != null)
                {
                    if (_dbInstance.Database.Connection.State == System.Data.ConnectionState.Open)
                    {
                        return _dbInstance;
                    }
                    else
                    {
                        _dbInstance.Database.Connection.Open();
                        return _dbInstance;
                    }
                }
                else
                {
                    _dbInstance = new HCContext();
                    return _dbInstance;
                }
            }
        }

    }
}
