﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace BulkWriter.Pipeline.Internal
{
    internal class ProjectEtlPipelineStep<TIn, TOut> : EtlPipelineStep<TIn, TOut>
    {
        private readonly Func<TIn, TOut> _projectionFunc;

        public ProjectEtlPipelineStep(EtlPipelineStepBase<TIn> previousStep, Func<TIn, TOut> projectionFunc) : base(previousStep)
        {
            _projectionFunc = projectionFunc ?? throw new ArgumentNullException(nameof(projectionFunc));
        }

        protected override Task RunCore(CancellationToken cancellationToken)
        {
            var enumerable = InputCollection.GetConsumingEnumerable(cancellationToken);

            foreach (var item in enumerable)
            {
                var result = _projectionFunc(item);
                OutputCollection.Add(result, cancellationToken);
            }

            return Task.CompletedTask;
        }
    }
}
