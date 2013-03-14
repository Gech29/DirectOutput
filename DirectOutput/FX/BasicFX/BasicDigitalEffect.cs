﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DirectOutput.Cab.Toys;
using DirectOutput.Table;

namespace DirectOutput.FX.BasicFX
{
    /// <summary>
    /// The <cref="BasicDigitalEffect"/> is used to set the state of toys implementing <see cref="IDigitalToy"/> based on the value of a <see cref="TableElement"/>.<br/>
    /// </summary>
    public class BasicDigitalEffect : EffectBase, IEffect
    {
        private string _DigitalToyName;

        /// <summary>
        /// Name of the <see cref="IDigitalToy"/>.
        /// </summary>
        /// <value>Name of the <see cref="IDigitalToy"/>.</value>
        public string DigitalToyName
        {
            get { return _DigitalToyName; }
            set
            {
                if (_DigitalToyName != value)
                {
                    _DigitalToyName = value;
                    _DigitalToy = null;
                }
            }
        }

        private void ResolveName(Pinball Pinball)
        {
            if (!DigitalToyName.IsNullOrWhiteSpace() && Pinball.Cabinet.Toys.Contains(DigitalToyName))
            {
                if (Pinball.Cabinet.Toys[DigitalToyName] is IDigitalToy)
                {
                    _DigitalToy = (IDigitalToy)Pinball.Cabinet.Toys[DigitalToyName];
                }
            }
        }

        private IDigitalToy _DigitalToy;

        /// <summary>
        /// Reference to the <see cref="IDigitalToy"/> specified in the <paramref name="DigitalToyName"/> property.<br/>
        /// If the  <paramref name="DigitalToyName"/> property is empty or contains a unknown name or the name of a toy which does not implement <see cref="IDigitalToy"/> this property will return null.
        /// </summary>
        public IDigitalToy DigitalToy
        {
            get
            {
                return _DigitalToy;
            }
        }


        /// <summary>
        /// Triggers the effect.<br/>
        /// If the value of the TableElement is 0 the <see cref="IDigitalToy"/> will be turned off, if the value is not equal 0 the <see cref="IDigitalToy"/> will be turned on.
        /// If TableElement is null, the State of the IDigitalToy will be set to true.
        /// </summary>
        public override void Trigger(TableElement TableElement)
        {
            if (DigitalToy != null)
            {
                if (TableElement != null)
                {
                    DigitalToy.State = (TableElement.Value == 0 ? false : true);
                }
                else
                {
                    DigitalToy.State = true;
                }
            }
        }

        /// <summary>
        /// Initializes the <see cref="IDigitalToy"/>.
        /// </summary>
        public override void Init(Pinball Pinball)
        {

            ResolveName(Pinball);
            if (DigitalToy != null) DigitalToy.Reset();
        }
        /// <summary>
        /// Finishes the <see cref="IDigitalToy"/>.
        /// </summary>
        public override void Finish()
        {
            if (DigitalToy != null) DigitalToy.Reset();
            _DigitalToy = null;
        }
    }
}
