using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DistributedAlgorithms.Algorithms.Base.Base;

namespace DistributedAlgorithms
{
    public class BaseAlgorithmHandler:AttributeList
    {
        private enum Comps { Trigger, Message}
        public BaseAlgorithmHandler():base()
        { }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// \fn public static ElementWindowPrms BaseAlgorithmPrms(Attribute attribute, dynamic key, NetworkElement mainNetworkElement, NetworkElement.ElementDictionaries mainDictionary, NetworkElement.ElementDictionaries dictionary, InputWindows inputWindow, bool windowEditable)
        ///
        /// \brief Base algorithm list prms.
        ///
        /// \par Description.
        ///      -  This method defines the ElementWindowPrms for the InternalEvent List in the GUI
        ///      -  The role of this method is to replace the regular method for adding attribute with
        ///         the method CreateDefaultEvent.
        ///
        /// \par Algorithm.
        ///
        /// \par Usage Notes.
        ///
        /// \author Ilanh
        /// \date 16/01/2018
        ///
        /// \param attribute          (Attribute) - The attribute.
        /// \param key                (dynamic) - The key.
        /// \param mainNetworkElement (NetworkElement) - The main network element.
        /// \param mainDictionary     (ElementDictionaries) - Dictionary of mains.
        /// \param dictionary         (ElementDictionaries) - The dictionary.
        /// \param inputWindow        (InputWindows) - The input window.
        /// \param windowEditable     (bool) - true if window editable.
        ///
        /// \return The ElementWindowPrms.
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static ElementWindowPrms BaseAlgorithmPrms(Attribute attribute,
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

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// \fn public static Attribute CreateDefaultEvent()
        ///
        /// \brief Creates default event.
        ///
        /// \par Description.
        ///      Used by the GUI(ElementWindow) to create a default event when pressing the Add Button
        ///
        /// \par Algorithm.
        ///
        /// \par Usage Notes.
        ///
        /// \author Ilanh
        /// \date 16/01/2018
        ///
        /// \return The new default event.
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static Attribute CreateDefaultEvent()
        {
            AttributeDictionary dict = new AttributeDictionary();
            dict.Add(Comps.Trigger, new EventTrigger());
            dict.Add(Comps.Message,  new BaseAlgorithmMessage());
            return new Attribute { Value = dict };            
        }

        public Attribute CreateEvent(EventTriggerType trigger,
            int round,
            dynamic eventMessageType,
            int eventMessageOtherEnd,
            dynamic baseMessageType,
            string baseMessageName,
            AttributeDictionary fields,
            AttributeList targets)
        {
            AttributeDictionary dict = new AttributeDictionary();
            dict.Add(Comps.Trigger, new EventTrigger(trigger, round, eventMessageType, eventMessageOtherEnd));
            dict.Add(Comps.Message, new BaseAlgorithmMessage(baseMessageType, baseMessageName, fields, targets));
            return new Attribute { Value = dict };
        }

        public void ProcessEvents(EventTriggerType triggerType, BaseMessage message)
        {
            foreach (Attribute attribute in this)
            {
                if (attribute.Value[Comps.Trigger].IsTrue(triggerType, message))
                {
                    attribute.Value[Comps.Message].Send((BaseProcess)Element, message.GetHeaderField(bm.pak.Round));
                }
            }
        }
    }
}
