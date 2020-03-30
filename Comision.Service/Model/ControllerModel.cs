using System.Collections.Generic;

namespace Comision.Service.Model
{
    public class ControllerModel
    {
        /// <summary>
        /// Gets or sets the name of controller.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of controller.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the actions.
        /// </summary>
        /// <value>The actions.</value>
        public IEnumerable<ActionModel> Actions { get; set; }
    }
}
