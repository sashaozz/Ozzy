//https://netfx.codeplex.com/SourceControl/latest#Extensions/Reflector/Source/Reflect.cs
/* 
Copyright (c) 2010, NETFx
All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

* Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.

* Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.

* Neither the name of Clarius Consulting nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
namespace Ozzy.Core
{
    public static class Reflect<TTarget>
    {
        public static PropertyInfo GetProperty(Expression<Func<TTarget, object>> property)
        {
            var info = GetMemberInfo(property, false) as PropertyInfo;
            if (info == null)
            {
                throw new ArgumentException("Member is not a property");
            }

            return info;
        }

        internal static List<TTarget> GetEnumValues()
        {
            return Enum.GetValues(typeof(TTarget))
                .Cast<TTarget>()
                .ToList();
        }

        public static PropertyInfo GetProperty(Expression<Func<TTarget, object>> property, bool checkForSingleDot)
        {
            return GetMemberInfo(property, checkForSingleDot) as PropertyInfo;
        }

        public static MemberInfo GetMemberInfo(Expression member, bool checkForSingleDot)
        {
            if (member == null)
            {
                throw new ArgumentNullException(nameof(member));
            }

            var lambda = member as LambdaExpression;
            if (lambda == null)
            {
                throw new ArgumentException("Not a lambda expression", nameof(member));
            }

            MemberExpression memberExpr = null;

            // The Func<TTarget, object> we use returns an object, so first statement can be either 
            // a cast (if the field/property does not return an object) or the direct member access.
            if (lambda.Body.NodeType == ExpressionType.Convert)
            {
                // The cast is an unary expression, where the operand is the 
                // actual member access expression.
                memberExpr = ((UnaryExpression)lambda.Body).Operand as MemberExpression;
            }
            else if (lambda.Body.NodeType == ExpressionType.MemberAccess)
            {
                memberExpr = lambda.Body as MemberExpression;
            }

            if (memberExpr == null)
            {
                throw new ArgumentException("Not a member access", nameof(member));
            }

            if (checkForSingleDot)
            {
                if (memberExpr.Expression is ParameterExpression)
                {
                    return memberExpr.Member;
                }
                throw new ArgumentException("Argument passed contains more than a single dot which is not allowed: " + member, nameof(member));
            }

            return memberExpr.Member;
        }
    }
}
