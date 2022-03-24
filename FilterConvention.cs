using HotChocolate.Data;
using HotChocolate.Data.Filters;
using HotChocolate.Data.Filters.Expressions;
using HotChocolate.Language;
using HotChocolate.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace CustomFiltering
{
   
        public class CustomConvention : FilterConvention
        {
            protected override void Configure(IFilterConventionDescriptor descriptor)
            {
                descriptor.AddDefaults();
                descriptor.Provider(
                    new QueryableFilterProvider(
                        x => x
                            .AddDefaultFieldHandlers().AddFieldHandler<QueryableStringInvariantContainsHandler>()
                            .AddFieldHandler< QueryableStringInvariantEqualsHandler>()));
            }
        }

        public class QueryableStringInvariantEqualsHandler : QueryableStringOperationHandler
        {

            public QueryableStringInvariantEqualsHandler(InputParser inputParser) : base(inputParser)
            {

            }

            private static readonly MethodInfo _toLower = typeof(string).GetMethods().Single(
                 x => x.Name == nameof(string.ToLower) && x.GetParameters().Length == 0);

            protected override int Operation => DefaultFilterOperations.Equals;

            public override Expression HandleOperation(QueryableFilterContext context, IFilterOperationField field, IValueNode value, object parsedValue)
        {
            
            Expression property = context.GetInstance();
            if (parsedValue is null)
            {
                return Expression.Equal(property, Expression.Constant(null));
            }

            if (parsedValue is string str)
            {
                
                return Expression.Equal(
                    Expression.Call(property, _toLower),
                    Expression.Constant(str.ToLower()));
            }
           
            throw new InvalidOperationException();
        }

        }

        public class QueryableStringInvariantContainsHandler : QueryableStringOperationHandler
        {
            public QueryableStringInvariantContainsHandler(InputParser inputParser) : base(inputParser)
            {

            }
            private static readonly MethodInfo _contains = typeof(string).GetMethod("IndexOf",
                    new[] { typeof(string), typeof(StringComparison) });

            protected override int Operation => DefaultFilterOperations.Contains;
            public override Expression HandleOperation(QueryableFilterContext context, IFilterOperationField field, IValueNode value, object parsedValue)
        {
            Expression property = context.GetInstance();

            if (parsedValue is null)
            {
                return Expression.Equal(property, Expression.Constant(null));
            }
            if (parsedValue is string str)
            {
                return Expression.NotEqual(
                        Expression.Call(
                            property,
                            _contains,
                            Expression.Constant(str, typeof(string)),
                            Expression.Constant(StringComparison.InvariantCultureIgnoreCase, typeof(StringComparison))
                        ),
                        Expression.Constant(-1, typeof(int))
                    );
            }
            throw new InvalidOperationException();
        }
        }
}

