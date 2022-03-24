

      using HotChocolate;
      using System.Collections.Generic;
      using System;
      using System.ComponentModel.DataAnnotations;
      using HotChocolate.Types;
      using HotChocolate.Data;


      namespace CustomFiltering
      {
          
                public partial class GetPartSaleOrderInquiry
                {
                        [GraphQLName("saleOrderNo")]    
                        public string? saleOrderNo { get; set; }
                      
                        [GraphQLName("saleOrderStatus")]    
                        public string? saleOrderStatus { get; set; }
                     
                        [UseOffsetPaging(IncludeTotalCount =true)]
                        [HotChocolate.Data.UseFiltering]
                        [UseSorting]
                        [GraphQLName("partDetails")]
                        public ICollection<GetPartDetails>? partDetails { get; set; }
                      
                }
                public partial class GetPartDetails
                {
                        [GraphQLName("mrstatusArr")]    
                        public string? mrstatusArr { get; set; }
                       
                }
                public partial class getPartSaleOrderInquiry
                {
                            [UseOffsetPaging(IncludeTotalCount =true)]
                            [HotChocolate.Data.UseFiltering]
                            [UseSorting]
                            public ICollection<GetPartSaleOrderInquiry>? data { get; set; }
                           
                }
      
      }