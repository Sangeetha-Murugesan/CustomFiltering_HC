

  using HotChocolate.Resolvers;
  using Microsoft.Extensions.Configuration;
  using Newtonsoft.Json;
  using Newtonsoft.Json.Linq;
  using Microsoft.AspNetCore.Http;
  using System;
  using System.Collections.Generic;
  using System.Threading.Tasks;
  using System.Linq;
  using System.Collections;
  using HotChocolate;
  using StackExchange.Redis;
  using Microsoft.Extensions.Logging;
  
  namespace CustomFiltering
  {
 public class BasPSOHub_getpartSaleOrderInquiry { 

       public async Task<getPartSaleOrderInquiry> getpartSaleOrderInquiry(IResolverContext res)
        {
            try
            {
                var response = System.IO.File.ReadAllText("./SampleData.json");
                var data= (JsonConvert.DeserializeObject<getPartSaleOrderInquiry>(response));
                return data;

            }
            catch (Exception ex)
            {
                
                throw ex;
            }

        }      
    }
  
  }