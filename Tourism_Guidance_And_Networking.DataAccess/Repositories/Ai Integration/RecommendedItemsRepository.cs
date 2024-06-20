using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tourism_Guidance_And_Networking.Core.Interfaces.Ai_Inegration;
using Tourism_Guidance_And_Networking.Core.Models.AI_Integration;

namespace Tourism_Guidance_And_Networking.DataAccess.Repositories.Ai_Integration
{
    public class RecommendedItemsRepository : BaseRepository<RecommendedItems>, IRecommendedItemsRepository
    {
        public RecommendedItemsRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
