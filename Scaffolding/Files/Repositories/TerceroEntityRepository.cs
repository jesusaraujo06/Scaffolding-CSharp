using Dapper;
using SARA.Server.DTOs.Shared;
using SARA.Server.Models.Shared;
using SARA.Server.Repositories.Shared.Interfaces;
using SARA.Server.Shared;
using System.Data;

namespace SARA.Server.Repositories.Example
{
    public class TerceroEntityRepository : GenericRepository<TerceroEntityEntity>, ITerceroEntityRepository
    {
        public TerceroEntityRepository(IDbConnection dbConnection) : base(dbConnection)
        {

        }
    }
}