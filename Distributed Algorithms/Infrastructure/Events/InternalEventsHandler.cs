using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DistributedAlgorithms.Algorithms.Base.Base;

namespace DistributedAlgorithms
{
    public class InternalEventsHandler:AttributeList
    {
        private enum Comps { Trigger, Event }
        public InternalEventsHandler():base()
        { }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// \fn public static ElementWindowPrms InternalEventListPrms(Attribute attribute, dynamic key, NetworkElement mainNetworkElement, NetworkElement.ElementDictionaries mainDictionary, NetworkElement.ElementDictionaries dictionary, InputWindows inputWindow, bool windowEditable)
        ///
        /// \brief Internal event list prms.
        ///
        /// \par Description.
        ///      -  This method defines the ElementWindowPrms for the InternalEvent List in the GUI  
        ///      -  The role of this method is to replace the regular method for adding attribute with  
        ///         the method CreateDefaultEvent  
        ///
        /// \par Algorithm.
        ///
        /// \par Usage Notes.
        ///
        /// \author Ilanh
        /// \date 16/01/2018
        ///
        /// \param attribute           (Attribute) - The attribute.
        /// \param key                 (dynamic) - The key.
        /// \param mainNetworkElement  (NetworkElement) - The main network element.
        /// \param mainDictionary      (ElementDictionaries) - Dictionary of mains.
        /// \param dictionary          (ElementDictionaries) - The dictionary.
        /// \param inputWindow         (InputWindows) - The input window.
        /// \param windowEditable      (bool) - true if window editable.
        ///
        /// \return The ElementWindowPrms.
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static ElementWindowPrms InternalEventPrms(Attribute attribute,
            dynamic key,
            NetworkElement mainNetworkElement,
            NetworkElement.ElementDictionaries mainDictionary,
            NetworkElement.ElementDictionaries dictionary,
            InputWindows inputWindow,
            bool windowEditable)
        {

            ElementWindowPrms elementWindowPrms = new ElementWindowPrms();
            elementWindowPrms.newValueControlPrms.inputFieldType = InputFieldsType.AddRemovePanel;
            elementWindowPrms.newValueControlPrms.add_button_click = CreateDefaultEvent;
            return elementWindowPrms;
        }

        public static Attribute CreateDefaultEvent()
        {
            AttributeDictionary dict = new AttributeDictionary();
            dict.Add(Comps.Trigger, new EventTrigger());
            dict.Add(Comps.Event, new BaseAlgorithmMessage());
            return new Attribute { Value = dict };
        }

        public Attribute CreateEvent(EventTriggerType trigger,
            int round,
            dynamic eventMessageType,
            int eventMessageOtherEnd,
            AttributeList methods)
        {
            AttributeDictionary dict = new AttributeDictionary();
            dict.Add(Comps.Trigger, new EventTrigger(trigger, round, eventMessageType, eventMessageOtherEnd));
            dict.Add(Comps.Event, new InternalEvent(methods));
            return new Attribute { Value = dict };
        }

        public void ProcessEvents(EventTriggerType triggerType, BaseMessage message)
        {
            foreach (Attribute attribute in this)
            {
                if (attribute.Value[Comps.Trigger].IsTrue(triggerType, message))
                {
                    attribute.Value[Comps.Event].Perform(); 
                }
            }
        }
    }
}
