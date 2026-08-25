using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

namespace Minsa.VPA.Repositorio
{
    public static class RepositorioDeExtensiones
    {
        public static List<T> ConvertirLista<T>(this IEnumerable<T> elementos)
        {
            if (elementos == null)
                return null;
            return elementos as List<T> ?? elementos.ToList();
        }

        //public static string ObtieneValidacionOVFact(this string texto)
        //{
            
        //}
        public static decimal ObtenerCantidadDecimal(this string texto)
        {
            decimal cantidad;
            Decimal.TryParse(texto, out cantidad);
            //int cantidad;
            //int.TryParse(texto, out cantidad);

            return cantidad;
        }

        public static float ObtenerCantidadFloat(this string texto)
        {
            float cantidad;
            float.TryParse(texto, out cantidad);
            //int cantidad;
            //int.TryParse(texto, out cantidad);

            return cantidad;
        }

        public static int ObtenerCantidadEntera(this string texto)
        {
            int cantidad;
            int.TryParse(texto, out cantidad);
            //int cantidad;
            //int.TryParse(texto, out cantidad);

            return cantidad;
        }

        public static string ObtenerDescripcion(this Enum enumeracion)
        {
            var tipo = enumeracion.GetType();
            var informacion = tipo.GetMember(enumeracion.ToString());
            if (informacion.Length == 0)
                return String.Empty;
            var atributos = informacion[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (!atributos.Any())
                throw new Exception("No tiene atributo de descripción.");

            return ((DescriptionAttribute)atributos[0]).Description;
        }

        public static string ObtenerNombreDePropiedad<T>(Expression<Func<T, object>> selector)
        {
            return new PropertyPathVisitor().GetPropertyPath(selector);
        }

        public static string ObtenerNombreDePropiedad<T, TK>(Expression<Func<T, TK>> selector)
        {
            return new PropertyPathVisitor().GetPropertyPath(selector);
        }

        public static string ObtenerNombreDePropiedad<T>(this T elemento, Expression<Func<T, object>> selector) where T : class
        {
            return new PropertyPathVisitor().GetPropertyPath(selector);
        }

        public static string ObtenerPropiedadDeLambda(LambdaExpression expression)
        {
            string result = String.Empty;
            if (expression == null)
                return String.Empty;
            var memberExpr = expression.Body is UnaryExpression
            ? (expression.Body as UnaryExpression).Operand as MemberExpression
            : expression.Body as MemberExpression;
            if (memberExpr != null)
                if (expression.NodeType != ExpressionType.Parameter)
                    result = ObtenerPropiedadDeMiembro(memberExpr.Expression as MemberExpression) +
                             memberExpr.Member.Name;

            return result;
        }

        private static string ObtenerPropiedadDeMiembro(MemberExpression expression)
        {
            string result = String.Empty;
            if (expression == null)
                return String.Empty;
            if (expression.NodeType != ExpressionType.Parameter)
            {
                var current = expression.Member.Name;
                var next = ObtenerPropiedadDeMiembro(expression.Expression as MemberExpression) + current;
                result = !String.IsNullOrEmpty(next) ? next + "." + result : result;
            }
            else
                result = "";
            return result;
        }

        public static TProperty ObtenerValorDePropiedad<TModel, TProperty>(this TModel modelo,
            Expression<Func<TModel, TProperty>> propiedad)
            where TProperty : IConvertible
        {
            return ObtenerValorDePropiedad<TModel, TProperty>(modelo, ObtenerPropiedadDeLambda(propiedad));
        }

        public static TProperty ObtenerValorDePropiedad<TModel, TProperty>(this TModel modelo, string propiedad)
        {
            return (TProperty)modelo.ObtenerInformacionDePropiedad(propiedad);
        }

        private static object ObtenerInformacionDePropiedad<T>(this T modelo, string destino)
        {
            object resultado = null;
            foreach (var propiedad in destino.Split('.'))
            {
                resultado = resultado != null
                    ? resultado.ObtenerValorDePropiedad(propiedad)
                    : modelo.ObtenerValorDePropiedad(propiedad);
            }

            return resultado;
        }

        private static object ObtenerValorDePropiedad<T>(this T modelo, string propiedad)
        {
            var tipo = modelo.GetType().GetGenericArguments().FirstOrDefault();
            return tipo != null
                ? tipo.GetProperty(propiedad).GetValue(modelo)
                : modelo.GetType().GetProperty(propiedad).GetValue(modelo);
        }

        public class PropertyPathVisitor : ExpressionVisitor
        {
            private Stack<string> _stack;

            public string GetPropertyPath(Expression expression)
            {
                _stack = new Stack<string>();
                Visit(expression);
                return _stack
                    .Aggregate(new StringBuilder(), (sb, name) => (sb.Length > 0 ? sb.Append(".") : sb).Append(name))
                    .ToString();
            }

            protected override Expression VisitMember(MemberExpression expression)
            {
                if (_stack != null)
                    _stack.Push(expression.Member.Name);
                return base.VisitMember(expression);
            }

            protected override Expression VisitMethodCall(MethodCallExpression expression)
            {
                if (IsLinqOperator(expression.Method))
                {
                    for (int i = 1; i < expression.Arguments.Count; i++)
                    {
                        Visit(expression.Arguments[i]);
                    }
                    Visit(expression.Arguments[0]);
                    return expression;
                }

                return base.VisitMethodCall(expression);
            }

            private static bool IsLinqOperator(MethodInfo method)
            {
                if (method.DeclaringType != typeof(Queryable) && method.DeclaringType != typeof(Enumerable))
                    return false;
                return Attribute.GetCustomAttribute(method, typeof(ExtensionAttribute)) != null;
            }

        }
    }
}