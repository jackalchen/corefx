// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Dynamic.Utils;
using System.Linq.Expressions;

namespace System.Dynamic
{
    /// <summary>
    /// Represents the unary dynamic operation at the call site, providing the binding semantic and the details about the operation.
    /// </summary>
    public abstract class UnaryOperationBinder : DynamicMetaObjectBinder
    {
        private ExpressionType _operation;

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryOperationBinder"/> class.
        /// </summary>
        /// <param name="operation">The unary operation kind.</param>
        protected UnaryOperationBinder(ExpressionType operation)
        {
            ContractUtils.Requires(OperationIsValid(operation), "operation");
            _operation = operation;
        }

        /// <summary>
        /// The result type of the operation.
        /// </summary>
        public override sealed Type ReturnType
        {
            get
            {
                switch (_operation)
                {
                    case ExpressionType.IsFalse:
                    case ExpressionType.IsTrue:
                        return typeof(bool);
                    default:
                        return typeof(object);
                }
            }
        }

        /// <summary>
        /// The unary operation kind.
        /// </summary>
        public ExpressionType Operation
        {
            get
            {
                return _operation;
            }
        }

        /// <summary>
        /// Performs the binding of the unary dynamic operation if the target dynamic object cannot bind.
        /// </summary>
        /// <param name="target">The target of the dynamic unary operation.</param>
        /// <returns>The <see cref="DynamicMetaObject"/> representing the result of the binding.</returns>
        public DynamicMetaObject FallbackUnaryOperation(DynamicMetaObject target)
        {
            return FallbackUnaryOperation(target, null);
        }

        /// <summary>
        /// Performs the binding of the unary dynamic operation if the target dynamic object cannot bind.
        /// </summary>
        /// <param name="target">The target of the dynamic unary operation.</param>
        /// <param name="errorSuggestion">The binding result in case the binding fails, or null.</param>
        /// <returns>The <see cref="DynamicMetaObject"/> representing the result of the binding.</returns>
        public abstract DynamicMetaObject FallbackUnaryOperation(DynamicMetaObject target, DynamicMetaObject errorSuggestion);

        /// <summary>
        /// Performs the binding of the dynamic unary operation.
        /// </summary>
        /// <param name="target">The target of the dynamic operation.</param>
        /// <param name="args">An array of arguments of the dynamic operation.</param>
        /// <returns>The <see cref="DynamicMetaObject"/> representing the result of the binding.</returns>
        public sealed override DynamicMetaObject Bind(DynamicMetaObject target, DynamicMetaObject[] args)
        {
            ContractUtils.RequiresNotNull(target, "target");
            ContractUtils.Requires(args == null || args.Length == 0, "args");

            return target.BindUnaryOperation(this);
        }

        // this is a standard DynamicMetaObjectBinder
        internal override sealed bool IsStandardBinder
        {
            get
            {
                return true;
            }
        }

        internal static bool OperationIsValid(ExpressionType operation)
        {
            switch (operation)
            {
                case ExpressionType.Negate:
                case ExpressionType.UnaryPlus:
                case ExpressionType.Not:
                case ExpressionType.Decrement:
                case ExpressionType.Increment:
                case ExpressionType.OnesComplement:
                case ExpressionType.IsTrue:
                case ExpressionType.IsFalse:
                case ExpressionType.Extension:
                    return true;

                default:
                    return false;
            }
        }
    }
}
