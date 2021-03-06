﻿using System;

namespace AccidentalFish.Foundations.Runtime
{
    /// <summary>
    /// Describes the configuration of a component in the component host
    /// </summary>
    public class ComponentConfiguration
    {
        /// <summary>
        /// If this is specified then the supplied factory will be used to instantiate the component
        /// </summary>
        public Func<IHostableComponent> Factory { get; set; }

        /// <summary>
        /// Number of instances to run in parallel
        /// </summary>
        public int Instances { get; set; }

        /// <summary>
        /// Function to determins if the component should be restarted. Passed the exception that caused the
        /// failure, the number of times the component has been restarted previously, and should return true
        /// for restart false if not.
        /// 
        /// If null then the default restart handler will be used. This will log the error and restart the
        /// component pausing for 30 seconds on every fifth error.
        /// </summary>
        public Func<Exception, int, bool> RestartEvaluator { get; set; }

        /// <summary>
        /// Name of the component - useful to identify it in logging and other diagnostic scenarios
        /// </summary>
        public string Name { get; set; }
    }
}
