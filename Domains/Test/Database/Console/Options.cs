namespace Allors
{
    /// <summary>
    /// The options.
    /// </summary>
    public enum Options
    {
        /// <summary>
        /// Saves the current population to population.xml
        /// </summary>
        Save, 

        /// <summary>
        /// Loads a the population from population.xml
        /// </summary>
        Load, 

        /// <summary>
        /// Creates a new population
        /// </summary>
        Populate, 

        /// <summary>
        /// Upgrades the current population
        /// </summary>
        Upgrade, 

        /// <summary>
        /// Demo
        /// </summary>
        Demo, 

        /// <summary>
        /// Report
        /// </summary>
        Report,

        /// <summary>
        /// Investigate
        /// </summary>
        Investigate,

        /// <summary>
        /// Exist the application
        /// </summary>
        Exit
    }
}