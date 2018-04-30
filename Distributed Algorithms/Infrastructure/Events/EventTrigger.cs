////////////////////////////////////////////////////////////////////////////////////////////////////
/// \file infrastructure\event.cs
///
/// \brief Implements the Event class.
////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using DistributedAlgorithms.Algorithms.Base.Base;

namespace DistributedAlgorithms
{
    public enum EventTriggerType
    {
        Initialize, BeforeSendMessage, AfterSendMessage, BeforeReceiveMessage, AfterReceiveMessage, UserEvent
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// \class Event
    ///
    /// \brief An event.
    ///
    /// \par Description.
    ///      -  Events are actions to perform when conditions are satisfied.  
    ///      -  There are 2 kinds of events  
    ///         -#  An internalEvent which is implemented by method
    ///         -#  A BaseAlgorithmEvent which is implementing a send of message of the base algorithm
    ///      -  The events has 2 parts  
    ///         -#  The condition which is held in this class. The role of the condition is to decide
    ///             whether to perform the event
    ///         -#  The action which is implemented by the inherited classes that specify the action to perform
    ///      -  This class has 2 roles:  
    ///         -#  Implement the condition
    ///         -#  Hold a virtual methods for implementation the actions   
    ///
    /// \par Usage Notes.
    ///
    /// \author Ilanh
    /// \date 16/01/2018
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public class EventTrigger : AttributeDictionary
    {
        #region /// \name Enums

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// \enum EventTrigger
        ///
        /// \brief Values that represent event triggers.
        ///        These keys are used to pass the status of the program for the check of the conditions
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// \enum EventTriggerData
        ///
        /// \brief Values that represent event trigger data
        ///        The conditions for activating the action are set in a dictionary that has the following keys.
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public enum Comps
        {
            Trigger, Round, MessageType, OtherEnd
        }
        #endregion
        #region /// \name Members

        /// \brief  (BaseProcess) - The process.
        protected BaseProcess process;
        #endregion
        #region /// \name Constructor

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// \fn public Event(BaseProcess process)
        ///
        /// \brief Constructor.
        ///
        /// \par Description.
        ///
        /// \par Algorithm.
        ///
        /// \par Usage Notes.
        ///
        /// \author Ilanh
        /// \date 16/01/2018
        ///
        /// \param process  (BaseProcess) - The process.
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public EventTrigger(EventTriggerType triggerType,
            int round,
            dynamic messageType,
            int messageOtherEnd) : base()
        {
            Add(Comps.Trigger, new Attribute { Value = triggerType, EndInputOperation = EventTriggerChanged });
            Add(Comps.Round, new Attribute { Value = round, ElementWindowPrmsMethod = DisableIfTriggerInitialize });
            Add(Comps.MessageType, new Attribute { Value = messageType, ElementWindowPrmsMethod = DisableIfTriggerInitialize });
            Add(Comps.OtherEnd, new Attribute { Value = messageOtherEnd, ElementWindowPrmsMethod = DisableIfTriggerInitialize });
        }
        public EventTrigger() : base()
        {
            Type messageTypeEnum = ClassFactory.GenerateMessageTypeEnum();
            dynamic messageType = Enum.ToObject(messageTypeEnum, 0);
            Add(Comps.Trigger, new Attribute { Value = EventTriggerType.AfterSendMessage, EndInputOperation = EventTriggerChanged });
            Add(Comps.Round, new Attribute { Value = 0, ElementWindowPrmsMethod = DisableIfTriggerInitialize });
            Add(Comps.MessageType, new Attribute { Value = messageType, ElementWindowPrmsMethod = DisableIfTriggerInitialize });
            Add(Comps.OtherEnd, new Attribute { Value = 0, ElementWindowPrmsMethod = DisableIfTriggerInitialize });
        }
        #endregion
        public bool True(EventTriggerType trigger, BaseMessage message)
        {
            switch (trigger)
            {
                case EventTriggerType.Initialize:
                    return IsInit();
                    break;
                default:
                    return IsTrue(trigger, message);
                    break;
            }
        }

        private bool IsInit()
        {
            return this[Comps.Trigger] == EventTriggerType.Initialize;
        }

        private bool IsTrue(EventTriggerType trigger, BaseMessage message)
        {
            // Get the data of the message that caused the event
            int messageRound = message.GetHeaderField(bm.pak.Round);
            dynamic messageType = message.GetHeaderField(bm.pak.MessageType);

            // Get the other end of the message that caused the event:
            // For a sending event this is the destination of the message
            // For a receive event this is the source of the message
            int messageOtherEnd;
            if (trigger == EventTriggerType.AfterSendMessage || trigger == EventTriggerType.BeforeSendMessage)
            {
                messageOtherEnd = message.GetHeaderField(bm.pak.DestProcess);
            }
            else
            {
                messageOtherEnd = message.GetHeaderField(bm.pak.SourceProcess);
            }

            if (this[Comps.Trigger] == trigger &&
                    this[Comps.Round] == messageRound &&
                    TypesUtility.CompareDynamics(this[Comps.MessageType], messageType) &&
                    this[Comps.OtherEnd] == messageOtherEnd)
            {
                return true;
            }
            return false;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////
        ///// \fn public static bool EventTriggerChanged(BaseNetwork network, NetworkElement networkElement, Attribute parentAttribute, Attribute attribute, string newValue, out string errorMessage, ElementWindow inputWindow = null)
        /////
        ///// \brief Event trigger changed.
        /////
        ///// \par Description.
        /////      Handle change of an event trigger - if the trigger is Initialize all the rest of
        /////      the parameters for the event get default value because they are not relevant
        /////
        ///// \par Algorithm.
        /////
        ///// \par Usage Notes.
        /////
        ///// \author Ilanh
        ///// \date 16/04/2018
        /////
        ///// \param       network          (BaseNetwork) - The network.
        ///// \param       networkElement   (NetworkElement) - The network element.
        ///// \param       parentAttribute  (Attribute) - The parent attribute.
        ///// \param       attribute        (Attribute) - The attribute.
        ///// \param       newValue         (string) - The new value.
        ///// \param [out] errorMessage    (out string) - Message describing the error.
        ///// \param       inputWindow     (Optional)  (ElementWindow) - The input window.
        /////
        ///// \return True if it succeeds, false if it fails.
        //////////////////////////////////////////////////////////////////////////////////////////////////////

        public static bool EventTriggerChanged(BaseNetwork network, NetworkElement networkElement, Attribute parentAttribute, Attribute attribute, string newValue, out string errorMessage, ElementWindow inputWindow = null)
        {
            // Get All the childes of the parent attribute
            List<ElementWindow.ControlsAttributeLink> links = inputWindow.controlsAttributeLinks.Values.Where(l => l.parentAttribute == parentAttribute
                && l.attribute != attribute).ToList();
            if (newValue == "Initialize")
            {
                foreach (ElementWindow.ControlsAttributeLink link in links)
                {
                    link.newValueControl.IsEnabled = false;
                    ((Control)link.newValueControl).Background = Brushes.LightGray;
                    string defaultValue = "";
                    inputWindow.ChangeValue(defaultValue, link.attribute);
                }
            }
            else
            {
                string prevValue = inputWindow.controlsAttributeLinks.Values.First(l => l.attribute == attribute).existingNewValue;
                if (prevValue == "Initialize")
                {
                    foreach (ElementWindow.ControlsAttributeLink link in links)
                    {
                        link.newValueControl.IsEnabled = true;
                        ((Control)link.newValueControl).Background = Brushes.White;
                        inputWindow.ChangeValue(link.existingValueTextBox.Text, link.attribute);
                    }
                }
            }
            errorMessage = "";
            return true;
        }

        public static ElementWindowPrms DisableIfTriggerInitialize(Attribute attribute, dynamic key,
            NetworkElement mainNetworkElement,
            NetworkElement.ElementDictionaries mainDictionary,
            NetworkElement.ElementDictionaries dictionary,
            InputWindows inputWindow,
            bool windowEdittable)
        {
            ElementWindowPrms prms = new ElementWindowPrms();
            attribute.DefaultNewValueFieldData(ref prms,
                mainNetworkElement,
                mainDictionary,
                dictionary,
                inputWindow,
                windowEdittable,
                attribute.Editable);
            if (((AttributeDictionary)attribute.Parent)[Comps.Trigger] == EventTriggerType.Initialize)
            {
                prms.newValueControlPrms.enable = false;
            }
            else
            {
                prms.newValueControlPrms.enable = true;
            }
            return prms;
        }
    }
}
//#region /// \name Creating a new event (Init or Design phase)

//////////////////////////////////////////////////////////////////////////////////////////////////////
///// \fn public static Attribute CreateEvent(EventTrigger eventTrigger, int eventRound, dynamic eventMessageType, int eventMessageOtherEnd)
/////
///// \brief Creates an event.
/////
///// \par Description.
/////      Create an event
/////      -  When creating an event the 2 parts of the event has to be filled  
/////      -  The following is the process of creating a new event  
/////         -#  The inherited class's CreateEvent method is called
/////         -#  The method calls this method
/////         -#  This method creates the following:
/////             -#  AttributeDictionary for the event
/////                 -#  Inside it AttributeDictionary for the conditions
/////                 -#  Fill the last AttributeDictionary
/////             -#  Return the first AttributeDictionary
/////         -#  The Create method of the inherited class creates and fills AttributeDictionary for the action
/////
///// \par Algorithm.
/////
///// \par Usage Notes.
/////
///// \author Ilanh
///// \date 16/01/2018
/////
///// \param eventTrigger          (EventTrigger) - The event trigger.
///// \param eventRound            (int) - The event round.
///// \param eventMessageType      (dynamic) - Type of the event message.
///// \param eventMessageOtherEnd  (int) - The event message other end.
/////
///// \return The new event.
//////////////////////////////////////////////////////////////////////////////////////////////////////

//public static Attribute CreateEvent(EventTrigger eventTrigger,
//    int eventRound,
//    dynamic eventMessageType,
//    int eventMessageOtherEnd)
//{
//    AttributeDictionary eventDictionary = new AttributeDictionary();
//    AttributeDictionary triggerDictionary = new AttributeDictionary();
//    Attribute attribute = new Attribute { Value = eventDictionary, ElementWindowPrmsMethod = MainDictionaryPrms};
//    eventDictionary.Add(EventParts.EventTrigger, new Attribute { Value = triggerDictionary, Editable=false, ElementWindowPrmsMethod = SubDictionaryPrms });

//    return attribute;

//}

//////////////////////////////////////////////////////////////////////////////////////////////////////
///// \fn public static bool EventTriggerChanged(BaseNetwork network, NetworkElement networkElement, Attribute parentAttribute, Attribute attribute, string newValue, out string errorMessage, ElementWindow inputWindow = null)
/////
///// \brief Event trigger changed.
/////
///// \par Description.
/////      Handle change of an event trigger - if the trigger is Initialize all the rest of
/////      the parameters for the event get default value because they are not relevant
/////
///// \par Algorithm.
/////
///// \par Usage Notes.
/////
///// \author Ilanh
///// \date 16/04/2018
/////
///// \param       network          (BaseNetwork) - The network.
///// \param       networkElement   (NetworkElement) - The network element.
///// \param       parentAttribute  (Attribute) - The parent attribute.
///// \param       attribute        (Attribute) - The attribute.
///// \param       newValue         (string) - The new value.
///// \param [out] errorMessage    (out string) - Message describing the error.
///// \param       inputWindow     (Optional)  (ElementWindow) - The input window.
/////
///// \return True if it succeeds, false if it fails.
//////////////////////////////////////////////////////////////////////////////////////////////////////

//public static bool EventTriggerChanged(BaseNetwork network, NetworkElement networkElement, Attribute parentAttribute, Attribute attribute, string newValue, out string errorMessage, ElementWindow inputWindow = null)
//{
//    // Get All the childes of the parent attribute
//    List<ElementWindow.ControlsAttributeLink> links = inputWindow.controlsAttributeLinks.Values.Where(l => l.parentAttribute == parentAttribute
//        && l.attribute != attribute).ToList();
//    if (newValue == "Initialize")
//    {
//        foreach (ElementWindow.ControlsAttributeLink link in links)
//        {
//            link.newValueControl.IsEnabled = false;
//            ((Control)link.newValueControl).Background = Brushes.LightGray;
//            string defaultValue = "";
//            inputWindow.ChangeValue(defaultValue, link.attribute);
//        }
//    }
//    else
//    {
//        string prevValue = inputWindow.controlsAttributeLinks.Values.First(l => l.attribute == attribute).existingNewValue;
//        if (prevValue == "Initialize")
//        {
//            foreach (ElementWindow.ControlsAttributeLink link in links)
//            {
//                link.newValueControl.IsEnabled = true;
//                ((Control)link.newValueControl).Background = Brushes.White;
//                inputWindow.ChangeValue(link.existingValueTextBox.Text, link.attribute);
//            }
//        }
//    }
//    errorMessage = "";
//    return true;
//}

//////////////////////////////////////////////////////////////////////////////////////////////////////
///// \fn public static void DisableIfTriggerInitialize(Attribute attribute, dynamic key, NetworkElement mainNetworkElement, NetworkElement.ElementDictionaries mainDictionary, NetworkElement.ElementDictionaries dictionary, InputWindows inputWindow, bool windowEdittable)
/////
///// \brief Disables if trigger initialize.
/////
///// \par Description.
/////      This method is used in order to disable all the fields of the event in case
/////      that the EventType is Initialize
/////
///// \par Algorithm.
/////
///// \par Usage Notes.
/////
///// \author Ilanh
///// \date 29/04/2018
/////
///// \param attribute           (Attribute) - The attribute.
///// \param key                 (dynamic) - The key.
///// \param mainNetworkElement  (NetworkElement) - The main network element.
///// \param mainDictionary      (ElementDictionaries) - Dictionary of mains.
///// \param dictionary          (ElementDictionaries) - The dictionary.
///// \param inputWindow         (InputWindows) - The input window.
///// \param windowEdittable     (bool) - true if window edittable.
//////////////////////////////////////////////////////////////////////////////////////////////////////

//public static ElementWindowPrms DisableIfTriggerInitialize(Attribute attribute, dynamic key,
//    NetworkElement mainNetworkElement,
//    NetworkElement.ElementDictionaries mainDictionary,
//    NetworkElement.ElementDictionaries dictionary,
//    InputWindows inputWindow,
//    bool windowEdittable)
//{
//    ElementWindowPrms prms = new ElementWindowPrms();
//    attribute.DefaultNewValueFieldData(ref prms,
//        mainNetworkElement,
//        mainDictionary,
//        dictionary,
//        inputWindow,
//        windowEdittable,
//        attribute.Editable);
//    if (((AttributeDictionary)attribute.Parent)[EventTriggerData.EventTrigger] == EventTrigger.Initialize)
//    {
//        prms.newValueControlPrms.enable = false;
//    }
//    else
//    {
//        prms.newValueControlPrms.enable = true;
//    }
//    return prms;
//}
//#endregion
//        #region /// \name Processing the events (Running phase)

//        ////////////////////////////////////////////////////////////////////////////////////////////////////
//        /// \fn public virtual void ProcessEvents(EventTrigger trigger, BaseMessage message, AttributeList eventsList)
//        ///
//        /// \brief Process the events.
//        ///
//        /// \par Description.
//        ///
//        /// \par Algorithm.
//        ///
//        /// \par Usage Notes.
//        ///
//        /// \author Ilanh
//        /// \date 16/04/2018
//        ///
//        /// \param trigger     (EventTrigger) - The trigger.
//        /// \param message     (BaseMessage) - The message.
//        /// \param eventsList  (AttributeList) - List of events.
//        ////////////////////////////////////////////////////////////////////////////////////////////////////

//        public virtual void ProcessEvents(EventTrigger trigger, BaseMessage message, AttributeList eventsList)
//        {
//            switch (trigger)
//            {
//                case EventTrigger.Initialize:
//                    ProcessInitEvents(trigger, eventsList);
//                    break;
//                default:
//                    ProcessMessageEvents(trigger, message, eventsList);
//                    break;
//            }
//        }




//        ////////////////////////////////////////////////////////////////////////////////////////////////////
//        /// \fn public virtual void ProcessEvents(EventTrigger trigger, BaseMessage message, AttributeList eventsList)
//        ///
//        /// \brief Process the events.
//        ///
//        /// \par Description.
//        ///      -  This method is used during running to process the events  
//        ///      -  It checks if the conditions a satisfied and if so it calls the PerformAction of the inherited
//        ///
//        /// \par Algorithm.
//        ///
//        /// \par Usage Notes.
//        ///
//        /// \author Ilanh
//        /// \date 16/01/2018
//        ///
//        /// \param trigger     (EventTrigger) - The trigger.
//        /// \param message     (BaseMessage) - The message.
//        /// \param eventsList  (AttributeList) - List of events.
//        ////////////////////////////////////////////////////////////////////////////////////////////////////

//        public virtual void ProcessMessageEvents(EventTrigger trigger, BaseMessage message, AttributeList eventsList)
//        {
//            // Get the data of the message that caused the event
//            int messageRound = message.GetHeaderField(bm.pak.Round);
//            dynamic messageType = message.GetHeaderField(bm.pak.MessageType);

//            // Get the other end of the message that caused the event:
//            // For a sending event this is the destination of the message
//            // For a receive event this is the source of the message
//            int messageOtherEnd;
//            if (trigger == EventTrigger.AfterSendMessage || trigger == EventTrigger.BeforeSendMessage)
//            {
//                messageOtherEnd = message.GetHeaderField(bm.pak.DestProcess);
//            }
//            else
//            {
//                messageOtherEnd = message.GetHeaderField(bm.pak.SourceProcess);
//            }


//            // If exists a record that fulfill the event conditions - send a base message 
//            for (int idx = 0; idx < eventsList.Count; idx++)
//            {
//                AttributeDictionary eventTriger = eventsList[idx][EventParts.EventTrigger];
//                if (eventTriger[EventTriggerData.EventTrigger] == trigger &&
//                    eventTriger[EventTriggerData.EventRound] == messageRound &&
//                    TypesUtility.CompareDynamics(eventTriger[EventTriggerData.EventMessageType], messageType) &&
//                    eventTriger[EventTriggerData.EventOtherEnd] == messageOtherEnd)
//                {
//                    IValueHolder eventAction = eventsList[idx][EventParts.EventAction];
//                    PerformAction(eventAction, messageRound);
//                }
//            }
//        }

//        ////////////////////////////////////////////////////////////////////////////////////////////////////
//        /// \fn public virtual void ProcessInitEvents(EventTrigger trigger, AttributeList eventsList)
//        ///
//        /// \brief Process the init events.
//        ///
//        /// \par Description.
//        ///      Process events before the start of the algorithm
//        ///
//        /// \par Algorithm.
//        ///
//        /// \par Usage Notes.
//        ///
//        /// \author Ilanh
//        /// \date 16/04/2018
//        ///
//        /// \param trigger     (EventTrigger) - The trigger.
//        /// \param eventsList  (AttributeList) - List of events.
//        ////////////////////////////////////////////////////////////////////////////////////////////////////

//        public virtual void ProcessInitEvents(EventTrigger trigger, AttributeList eventsList)
//        {
//            for (int idx = 0; idx < eventsList.Count; idx++)
//            {
//                AttributeDictionary eventTriger = eventsList[idx][EventParts.EventTrigger];
//                if (eventTriger[EventTriggerData.EventTrigger] == trigger)
//                {
//                    IValueHolder eventAction = eventsList[idx][EventParts.EventAction];
//                    PerformAction(eventAction, 0);
//                }
//            }
//        }



//        ////////////////////////////////////////////////////////////////////////////////////////////////////
//        /// \fn protected virtual void PerformAction(IValueHolder eventAction, int round)
//        ///
//        /// \brief Performs the action.
//        ///
//        /// \par Description.
//        ///      Virtual method for performing the action - does nothing
//        ///
//        /// \par Algorithm.
//        ///
//        /// \par Usage Notes.
//        ///
//        /// \author Ilanh
//        /// \date 16/01/2018
//        ///
//        /// \param eventAction  (IValueHolder) - The event action.
//        /// \param round        (int) - The round.
//        ////////////////////////////////////////////////////////////////////////////////////////////////////

//        protected virtual void PerformAction(IValueHolder eventAction, int round)
//        { }
//        #endregion
//        #region /// \name ElementWindow support

//        ////////////////////////////////////////////////////////////////////////////////////////////////////
//        /// \fn public static ElementWindowPrms MainDictionaryPrms(Attribute attribute, dynamic key, NetworkElement mainNetworkElement, NetworkElement.ElementDictionaries mainDictionary, NetworkElement.ElementDictionaries dictionary, InputWindows inputWindow, bool windowEditable)
//        ///
//        /// \brief Main dictionary prms.
//        ///
//        /// \par Description.
//        ///      The ElementWindowPrms for the event main AttributeDictionary (A TextBox)
//        ///
//        /// \par Algorithm.
//        ///
//        /// \par Usage Notes.
//        ///
//        /// \author Ilanh
//        /// \date 16/01/2018
//        ///
//        /// \param attribute           (Attribute) - The attribute.
//        /// \param key                 (dynamic) - The key.
//        /// \param mainNetworkElement  (NetworkElement) - The main network element.
//        /// \param mainDictionary      (ElementDictionaries) - Dictionary of mains.
//        /// \param dictionary          (ElementDictionaries) - The dictionary.
//        /// \param inputWindow         (InputWindows) - The input window.
//        /// \param windowEditable      (bool) - true if window editable.
//        ///
//        /// \return The ElementWindowPrms.
//        ////////////////////////////////////////////////////////////////////////////////////////////////////

//        public static ElementWindowPrms MainDictionaryPrms(Attribute attribute,
//            dynamic key,
//            NetworkElement mainNetworkElement,
//            NetworkElement.ElementDictionaries mainDictionary,
//            NetworkElement.ElementDictionaries dictionary,
//            InputWindows inputWindow,
//            bool windowEditable)
//        {
//            ElementWindowPrms elementWindowPrms = new ElementWindowPrms();
//            elementWindowPrms.newValueControlPrms.inputFieldType = InputFieldsType.TextBox;
//            elementWindowPrms.newValueControlPrms.Value = "Event No." + key;
//            elementWindowPrms.newValueControlPrms.enable = false;
//            return elementWindowPrms;
//        }

//        ////////////////////////////////////////////////////////////////////////////////////////////////////
//        /// \fn public static ElementWindowPrms SubDictionaryPrms(Attribute attribute, dynamic key, NetworkElement mainNetworkElement, NetworkElement.ElementDictionaries mainDictionary, NetworkElement.ElementDictionaries dictionary, InputWindows inputWindow, bool windowEditable)
//        ///
//        /// \brief Sub dictionary prms.
//        ///
//        /// \par Description.
//        ///      The ElementWindowPrms foe each one of the 2 sub AttributeDictionary
//        ///
//        /// \par Algorithm.
//        ///
//        /// \par Usage Notes.
//        ///
//        /// \author Ilanh
//        /// \date 16/01/2018
//        ///
//        /// \param attribute           (Attribute) - The attribute.
//        /// \param key                 (dynamic) - The key.
//        /// \param mainNetworkElement  (NetworkElement) - The main network element.
//        /// \param mainDictionary      (ElementDictionaries) - Dictionary of mains.
//        /// \param dictionary          (ElementDictionaries) - The dictionary.
//        /// \param inputWindow         (InputWindows) - The input window.
//        /// \param windowEditable      (bool) - true if window editable.
//        ///
//        /// \return The ElementWindowPrms.
//        ////////////////////////////////////////////////////////////////////////////////////////////////////

//        public static ElementWindowPrms SubDictionaryPrms(Attribute attribute,
//            dynamic key,
//            NetworkElement mainNetworkElement,
//            NetworkElement.ElementDictionaries mainDictionary,
//            NetworkElement.ElementDictionaries dictionary,
//            InputWindows inputWindow,
//            bool windowEditable)
//        {
//            ElementWindowPrms elementWindowPrms = new ElementWindowPrms();
//            elementWindowPrms.newValueControlPrms.inputFieldType = InputFieldsType.TextBox;
//            elementWindowPrms.newValueControlPrms.Value = TypesUtility.GetKeyToString(key);
//            elementWindowPrms.newValueControlPrms.enable = false;
//            return elementWindowPrms;
//        }
//        #endregion
//    }
//}
