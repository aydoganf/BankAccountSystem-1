using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AydoganFBank.Context.Query
{
    public static class Helpers
    {
        public static Expression<Func<TInput, bool>> CombineWithAnd<TInput>(this Expression<Func<TInput, bool>> expression1, Expression<Func<TInput, bool>> expression2)
        {
            return Expression.Lambda<Func<TInput, bool>>(
                Expression.AndAlso(expression1.Body, new ExpressionParameterReplacer(expression2.Parameters, expression1.Parameters).Visit(expression2.Body)), expression1.Parameters);
        }

        private class ExpressionParameterReplacer : ExpressionVisitor
        {
            public ExpressionParameterReplacer(IList<ParameterExpression> fromParameters, IList<ParameterExpression> toParameters)
            {
                ParameterReplacements = new Dictionary<ParameterExpression, ParameterExpression>();
                for (int i = 0; i != fromParameters.Count && i != toParameters.Count; i++)
                    ParameterReplacements.Add(fromParameters[i], toParameters[i]);
            }

            private IDictionary<ParameterExpression, ParameterExpression> ParameterReplacements { get; set; }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                ParameterExpression replacement;
                if (ParameterReplacements.TryGetValue(node, out replacement))
                    node = replacement;
                return base.VisitParameter(node);
            }
        }
    }
}
