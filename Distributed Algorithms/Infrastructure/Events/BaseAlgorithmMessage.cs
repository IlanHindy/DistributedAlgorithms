////////////////////////////////////////////////////////////////////////////////////////////////////
/// \file infrastructure\basealgorithmevent.cs
///
/// \brief Implements the BaseAlgorithmEvent class.
////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DistributedAlgorithms.Algorithms.Base.Base;

namespace DistributedAlgorithms
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// \class BaseAlgorithmEvent
    ///
    /// \brief A base algorithm event.
    ///
    /// \par Description.
    ///      -  Events are actions to perform when conditions are satisfied.
    ///      -  There are 2 kinds of events
    ///         -#  An internalEvent which is implemented by method
    ///         -#  A BaseAlgorithmEvent which is implementing a send of message of the base algorithm
    ///      -  The events has 2 parts
    ///         -#  The condition which is held in the Event. The role of the condition is to decide
    ///             whether to perform the event
    ///         -#  The action which is implemented by the inherited classes that specify the action to perform
    ///      -  This class has 2 roles:
    ///         -#  Holds the attributes needed in order to create the base algorithm  message
    ///         -#  Holds all the methods needed for the performing of the BaseAlgorithmEvent     
    ///
    /// \par Usage Notes.
    ///
    /// \author Ilanh
    /// \date 16/01/2018
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public class BaseAlgorithmMessage: AttributeDictionary
    {
        #region /// \name Enums
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// \enum EventAction
        ///
        /// \brief Values that represent event actions.
        ///        -    The data for creating a base algorithm message is found in an AttributeDictionary.   
        ///        -    This enum contains the keys to this dictionary
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public enum Comps { Type, Name, Fields, Targets }
        #endregion
        #region /// \name Constructor

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// \fn public BaseAlgorithmEvent(BaseProcess process) : base(process)
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

        public BaseAlgorithmMessage(dynamic type,
            string name,
            AttributeDictionary fields,
            AttributeList targets) : base()
        {
            Add(Comps.Type, new Attribute { Value = type });
            Add(Comps.Name, new Attribute { Value = name });
            Add(Comps.Fields, new Attribute { Value = fields });
            Add(Comps.Targets, new Attribute { Value = fields });
        }
        public BaseAlgorithmMessage():base()
        {
            Type messageTypeEnum = ClassFactory.GenerateMessageTypeEnum();
            dynamic type = Enum.ToObject(messageTypeEnum, 0);
            Add(Comps.Type, new Attribute { Value = type });
            Add(Comps.Name, new Attribute { Value = "BaseMessage" });
            Add(Comps.Fields, new Attribute { Value = new AttributeDictionary() });
            Add(Comps.Targets, new Attribute { Value = new AttributeList() });
        }

        public void Send(int round)
        {
            BaseProcess process = (BaseProcess)Element;
            BaseMessage message = process.BuildBaseAlgorithmMessage(
                this[Comps.Type],
                this[Comps.Name],
                this[Comps.Fields],
                round,
                this[Comps.Targets]);
            process.SendToNeighbours(message, BaseProcess.SelectingMethod.Include, this[Comps.Targets]);
            List<int> excludeProcesses = process.OutChannels.Select(c => (int)(c.ea[bc.eak.DestProcess])).ToList();
        }
        #endregion
        //#region /// \name Creating/inserting a new InternalEvent

        //////////////////////////////////////////////////////////////////////////////////////////////////////
        ///// \fn public void InsertEvent(Event.EventTrigger eventTrigger, int eventRound, dynamic eventMessageType, int eventMessageOtherEnd, dynamic baseMessageType, string baseMessageName, AttributeDictionary baseMessageFields, AttributeList targets)
        /////
        ///// \brief Inserts an event.
        /////
        ///// \par Description.
        /////      -  This method is activated by the processor to add a new BaseAlgorithmEvent
        /////      -  There are 2 ways to add BaseAlgorithmEvent. This method is used to add BaseAlgorithmEvent
        /////         programmatically
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
        ///// \param baseMessageType       (dynamic) - Type of the base message.
        ///// \param baseMessageName       (string) - Name of the base message.
        ///// \param baseMessageFields     (AttributeDictionary) - The base message fields.
        ///// \param targets               (AttributeList) - The targets.
        //////////////////////////////////////////////////////////////////////////////////////////////////////

        //public void InsertEvent(Event.EventTrigger eventTrigger,
        //    int eventRound,
        //    dynamic eventMessageType,
        //    int eventMessageOtherEnd,
        //    dynamic baseMessageType,
        //    string baseMessageName,
        //    AttributeDictionary baseMessageFields,
        //    AttributeList targets)
        //{
        //    AttributeDictionary eventDictionary = CreateEvent(eventTrigger,
        //        eventRound,
        //        eventMessageType,
        //        eventMessageOtherEnd,
        //        baseMessageType,
        //        baseMessageName,
        //        baseMessageFields,
        //        targets);
        //    process.op[bp.opk.BaseAlgorithmData].Add(new Attribute { Value = eventDictionary, Editable = false });
        //}

        //////////////////////////////////////////////////////////////////////////////////////////////////////
        ///// \fn public static Attribute CreateEvent(EventTrigger eventTrigger, int eventRound, dynamic eventMessageType, int eventMessageOtherEnd, dynamic baseMessageType, string baseMessageName, AttributeDictionary baseMessageFields, AttributeList targets)
        /////
        ///// \brief Creates an event.
        /////
        ///// \par Description.
        /////      This method is used to create an event in the following cases
        /////      -# Programatically (Called by InsertEvent)
        /////      -# From the GUI (called by CreateDefaultEvent)
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
        ///// \param baseMessageType       (dynamic) - Type of the base message.
        ///// \param baseMessageName       (string) - Name of the base message.
        ///// \param baseMessageFields     (AttributeDictionary) - The base message fields.
        ///// \param targets               (AttributeList) - The targets.
        /////
        ///// \return The new event.
        //////////////////////////////////////////////////////////////////////////////////////////////////////

        //public static Attribute CreateEvent(EventTrigger eventTrigger,
        //    int eventRound,
        //    dynamic eventMessageType,
        //    int eventMessageOtherEnd,
        //    dynamic baseMessageType,
        //    string baseMessageName,
        //    AttributeDictionary baseMessageFields,
        //    AttributeList targets)
        //{

        //    bool error = false;
        //    string prmString = "";

        //    // Check that the message type of the trigger and the action is from the message types of the algorithm
        //    Type messageTypeEnum = ClassFactory.GenerateMessageTypeEnum();
        //    string messageTypeEnumString = messageTypeEnum.ToString();
        //    if (!(baseMessageType.GetType().Equals(messageTypeEnum)))
        //    {
        //        error = true;
        //        prmString = "baseMessageType : " + baseMessageType.ToString();
        //    }
        //    if (!(eventMessageType.GetType().Equals(messageTypeEnum)))
        //    {
        //        error = true;
        //        prmString = "eventMessageType : " + baseMessageType.ToString();
        //    }

        //    if (error)
        //    {
        //        CustomizedMessageBox.Show(new List<String> { "Error In Inserting event to Base Algorithm :",
        //        "The parameter " + prmString + "Has to be from type " + messageTypeEnumString},
        //            "Add Base Algorithm Event error",
        //            null,
        //            Icons.Error);
        //        return null;
        //    }

        //    Attribute eventAttribute = CreateEvent(eventTrigger, eventRound, eventMessageType, eventMessageOtherEnd);
        //    AttributeDictionary actionDictionary = new AttributeDictionary();
        //    eventAttribute.Value.Add(EventParts.EventAction, new Attribute { Value = actionDictionary, ElementWindowPrmsMethod = SubDictionaryPrms });

        //    // Sending message parameters
        //    actionDictionary.Add(EventAction.BaseMessageType, new Attribute { Value = baseMessageType });
        //    actionDictionary.Add(EventAction.BaseMessageName, new Attribute { Value = baseMessageName });
        //    actionDictionary.Add(EventAction.BaseMessageFields, new Attribute { Value = baseMessageFields });
        //    actionDictionary.Add(EventAction.Targets, new Attribute { Value = targets, ElementWindowPrmsMethod = IntListPrms });

        //    return eventAttribute;
        //}
        //#endregion
        //#region /// \name Processing the events (Running phase) 
        //////////////////////////////////////////////////////////////////////////////////////////////////////
        ///// \fn public void ProcessEvents(EventTrigger trigger, BaseMessage message)
        /////
        ///// \brief Process the events.
        /////
        ///// \par Description.
        /////      -  Process the Base Algorithm events.
        /////      -  This method is used by the processor to check all the events and perform
        /////         the actions if the condition satisfies
        /////
        ///// \par Algorithm.
        /////
        ///// \par Usage Notes.
        /////
        ///// \author Ilanh
        ///// \date 16/01/2018
        /////
        ///// \param trigger  (EventTrigger) - The trigger.
        ///// \param message  (BaseMessage) - The message.
        //////////////////////////////////////////////////////////////////////////////////////////////////////

        //public void ProcessEvents(EventTrigger trigger, BaseMessage message)
        //{
        //    base.ProcessEvents(trigger, message, (AttributeList)process.op[bp.opk.BaseAlgorithmData]);
        //}

        //////////////////////////////////////////////////////////////////////////////////////////////////////
        ///// \fn protected override void PerformAction(IValueHolder eventAction, int round)
        /////
        ///// \brief Performs the action.
        /////
        ///// \par Description.
        /////      When an event conditions is satisfies a call is made to perform the InternalEvent handler
        /////
        ///// \par Algorithm.
        /////      -# Call the BuildBaseAlgorithmMessage to allow the processor add fields in run time
        /////      -3 Send the message to all the processors
        /////
        ///// \par Usage Notes.
        /////
        ///// \author Ilanh
        ///// \date 16/01/2018
        /////
        ///// \param eventAction  (IValueHolder) - The event action.
        ///// \param round        (int) - The round.
        //////////////////////////////////////////////////////////////////////////////////////////////////////

        //protected override void PerformAction(IValueHolder eventAction, int round)
        //{
        //    AttributeDictionary action = (AttributeDictionary)eventAction;
        //    BaseMessage message = process.BuildBaseAlgorithmMessage(
        //        action[EventAction.BaseMessageType],
        //        action[EventAction.BaseMessageName],
        //        action[EventAction.BaseMessageFields],
        //        round, 
        //        action[EventAction.Targets]);
        //    List<int> excludeProcesses = process.OutChannels.Select(c => (int)(c.ea[bc.eak.DestProcess])).ToList();
        //    ((AttributeList)action[EventAction.Targets]).ForEach(a => excludeProcesses.Remove(a.Value));
        //    process.SendToNeighbours(message, excludeProcesses);
        //}
        //#endregion
        //#region /// \name ElementWindow support

        //////////////////////////////////////////////////////////////////////////////////////////////////////
        ///// \fn public static ElementWindowPrms IntListPrms(Attribute attribute, dynamic key, NetworkElement mainNetworkElement, NetworkElement.ElementDictionaries mainDictionary, NetworkElement.ElementDictionaries dictionary, InputWindows inputWindow, bool windowEditable)
        /////
        ///// \brief Int list prms.
        /////
        ///// \par Description.
        /////      -  This method defines the ElementWindowPrms for the target process ids in the GUI
        /////      -  The role of this method is to replace the regular method for adding attribute with
        /////         the method CreateProcessIdAttribute
        /////
        ///// \par Algorithm.
        /////
        ///// \par Usage Notes.
        /////
        ///// \author Ilanh
        ///// \date 16/01/2018
        /////
        ///// \param attribute           (Attribute) - The attribute.
        ///// \param key                 (dynamic) - The key.
        ///// \param mainNetworkElement  (NetworkElement) - The main network element.
        ///// \param mainDictionary      (ElementDictionaries) - Dictionary of mains.
        ///// \param dictionary          (ElementDictionaries) - The dictionary.
        ///// \param inputWindow         (InputWindows) - The input window.
        ///// \param windowEditable      (bool) - true if window editable.
        /////
        ///// \return The ElementWindowPrms.
        //////////////////////////////////////////////////////////////////////////////////////////////////////

        //public static ElementWindowPrms IntListPrms(Attribute attribute,
        //    dynamic key,
        //           NetworkElement mainNetworkElement,
        //           NetworkElement.ElementDictionaries mainDictionary,
        //           NetworkElement.ElementDictionaries dictionary,
        //           InputWindows inputWindow,
        //           bool windowEditable)
        //{

        //    ElementWindowPrms elementWindowPrms = new ElementWindowPrms();
        //    elementWindowPrms.newValueControlPrms.inputFieldType = InputFieldsType.AddRemovePanel;
        //    elementWindowPrms.newValueControlPrms.add_button_click = CreateProcessIdAttribute;
        //    return elementWindowPrms;
        //}

        //////////////////////////////////////////////////////////////////////////////////////////////////////
        ///// \fn public static Attribute CreateProcessIdAttribute()
        /////
        ///// \brief Creates process identifier attribute.
        /////
        ///// \par Description.
        /////      This method is used in the GUI to add a new attribute holding the default process id
        /////      when the Add button of the target process list is pressed
        /////
        ///// \par Algorithm.
        /////
        ///// \par Usage Notes.
        /////
        ///// \author Ilanh
        ///// \date 16/01/2018
        /////
        ///// \return The new process identifier attribute.
        //////////////////////////////////////////////////////////////////////////////////////////////////////

        //public static Attribute CreateProcessIdAttribute()
        //{
        //    return new Attribute { Value = 0, EndInputOperation = CheckIfProcessExists };
        //}

        //////////////////////////////////////////////////////////////////////////////////////////////////////
        ///// \fn public static bool CheckIfProcessExists(BaseNetwork network, NetworkElement networkElement, Attribute parentAttribute, Attribute attribute, string newValue, out string errorMessage, ElementWindow inputWindow = null)
        /////
        ///// \brief Determine if process exists.
        /////
        ///// \par Description.
        /////      This method will be activated after a change in one of the target process will be
        /////      done in the GUI (ElementWindow) it checks if there is a process with the new process id
        /////
        ///// \par Algorithm.
        /////
        ///// \par Usage Notes.
        /////
        ///// \author Ilanh
        ///// \date 16/01/2018
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

        //public static bool CheckIfProcessExists(BaseNetwork network,
        //    NetworkElement networkElement,
        //    Attribute parentAttribute,
        //    Attribute attribute,
        //    string newValue,
        //    out string errorMessage,
        //    ElementWindow inputWindow = null)
        //{
        //    errorMessage = "";
        //    try
        //    {
        //        errorMessage = "The value has to be int";
        //        int id = int.Parse(newValue);

        //        errorMessage = "There is no processor with id :" + newValue;
        //        network.Processes.First(p => p.ea[ne.eak.Id] == id);

        //        errorMessage = "";
        //        return true;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}
        //#endregion
    }
}
