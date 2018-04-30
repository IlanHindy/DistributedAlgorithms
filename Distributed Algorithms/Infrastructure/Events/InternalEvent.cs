////////////////////////////////////////////////////////////////////////////////////////////////////
/// \file infrastructure\internalevent.cs
///
/// \brief Implements the internalevent class.
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
    /// \class InternalEvent
    ///
    /// \brief An internal event.
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
    ///         -#  Holds the names of methods that has to be activated when an Event occurs
    ///         -#  Holds all the methods needed for the performing of the InternalEvent
    ///        
    /// \par Usage Notes.
    ///
    /// \author Ilanh
    /// \date 16/01/2018
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public class InternalEvent : AttributeDictionary
    {
        #region /// \name Enums
        public enum InternalEventComps { EventAction }
        #endregion

        #region /// \name Delegates
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// \class DummyForInternalEvent
        ///
        /// \brief A dummy for internal event.
        ///
        /// \par Description.
        ///      -  This class is used to identify the methods that are used as event handlers  
        ///      -  The methods has this class as their first parameter so that the GUI when  
        ///         searching for all the methods will be able to identify them.
        ///         (Basically the action method should have no parameters. so when filling
        ///         the combo box for selecting the method in the GUI all the methods that has
        ///         no parameters will be shown this parameter is used to select only tha relevant methods)
        ///
        /// \par Usage Notes.
        ///
        /// \author Ilanh
        /// \date 16/01/2018
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public class DummyForInternalEvent { }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// \fn public delegate void InternalEventDelegate(DummyForInternalEvent dummy);
        ///
        /// \brief Internal event delegate.
        ///
        /// \author Ilanh
        /// \date 16/01/2018
        ///
        /// \param dummy  (DummyForInternalEvent) - The dummy.
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public delegate void InternalEventDelegate(DummyForInternalEvent dummy);
        #endregion
        #region /// \name Constructors
        public InternalEvent(AttributeList methods) : base()
        {
            Add(InternalEventComps.EventAction, new Attribute { Value = methods, ElementWindowPrmsMethod = MethodsListPrms });
        }
        public InternalEvent() : base()
        {
            Add(InternalEventComps.EventAction, new AttributeList());
        }

        protected void Perform(AttributeList methods, int round)
        {
            for (int idx = 0; idx < methods.Count; idx++)
            {
                TypesUtility.InvokeMethod(Element, methods[idx], new List<object>(), 1, false);
            }
        }

        public static ElementWindowPrms MethodsListPrms(Attribute attribute,
            dynamic key,
            NetworkElement mainNetworkElement,
            NetworkElement.ElementDictionaries mainDictionary,
            NetworkElement.ElementDictionaries dictionary,
            InputWindows inputWindow,
            bool windowEditable)
        {

            ElementWindowPrms elementWindowPrms = new ElementWindowPrms();
            elementWindowPrms.newValueControlPrms.inputFieldType = InputFieldsType.AddRemovePanel;
            elementWindowPrms.newValueControlPrms.add_button_click = AddMethod;
            return elementWindowPrms;
        }

        public static Attribute AddMethod()
        {
            return new Attribute { Value = "DefaultInternalEvent", ElementWindowPrmsMethod = MethodComboBoxPrms };
        }

        public static ElementWindowPrms MethodComboBoxPrms(Attribute attribute,
            dynamic key,
            NetworkElement mainNetworkElement,
            NetworkElement.ElementDictionaries mainDictionary,
            NetworkElement.ElementDictionaries dictionary,
            InputWindows inputWindow,
            bool windowEditable)
        {
            ElementWindowPrms elementWindowPrms = new ElementWindowPrms();
            elementWindowPrms.newValueControlPrms.inputFieldType = InputFieldsType.ComboBox;
            string[] options = TypesUtility.GetAllInternalEventMethods().ToArray();
            elementWindowPrms.newValueControlPrms.options = options;
            elementWindowPrms.newValueControlPrms.Value = attribute.Value;
            return elementWindowPrms;
        }
    }
}
        #endregion
        //#region /// \name Constructor

        //////////////////////////////////////////////////////////////////////////////////////////////////////
        ///// \fn public InternalEvent(BaseProcess process) : base(process)
        /////
        ///// \brief Constructor.
        /////
        ///// \par Description.
        /////
        ///// \par Algorithm.
        /////
        ///// \par Usage Notes.
        /////
        ///// \author Ilanh
        ///// \date 16/01/2018
        /////
        ///// \param process  (BaseProcess) - The process.
        //////////////////////////////////////////////////////////////////////////////////////////////////////

        //public InternalEvent(BaseProcess process) : base(process) { }
        //#endregion
        //#region /// \name Creating/inserting a new InternalEvent

        //////////////////////////////////////////////////////////////////////////////////////////////////////
        ///// \fn public void InsertEvent(Event.EventTrigger eventTrigger, int eventRound, dynamic eventMessageType, int eventMessageOtherEnd, AttributeList methods)
        /////
        ///// \brief Inserts an event.
        /////
        ///// \par Description.
        /////      -  This method is activated by the processor to add a new InternalEvent  
        /////      -  There are 2 ways to add InternalEvent. This method is used to add InternalEvent   
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
        ///// \param methods               (AttributeList) - The methods.
        //////////////////////////////////////////////////////////////////////////////////////////////////////

        //public void InsertEvent(EventTrigger eventTrigger,
        //    int eventRound,
        //    dynamic eventMessageType,
        //    int eventMessageOtherEnd,
        //    AttributeList methods)
        //{
        //    AttributeDictionary eventDictionary = CreateEvent(eventTrigger,
        //        eventRound,
        //        eventMessageType,
        //        eventMessageOtherEnd,
        //        methods);
        //    process.op[bp.opk.InternalEvents].Add(eventDictionary);
        //}

        //////////////////////////////////////////////////////////////////////////////////////////////////////
        ///// \fn public static Attribute CreateDefaultEvent()
        /////
        ///// \brief Creates default event.
        /////
        ///// \par Description.
        /////      Used by the GUI(ElementWindow) to create a default event when pressing the Add Button
        /////
        ///// \par Algorithm.
        /////
        ///// \par Usage Notes.
        /////
        ///// \author Ilanh
        ///// \date 16/01/2018
        /////
        ///// \return The new default event.
        //////////////////////////////////////////////////////////////////////////////////////////////////////

        //public static Attribute CreateDefaultEvent()
        //{
        //    Type messageTypeEnum = ClassFactory.GenerateMessageTypeEnum();
        //    dynamic messageType = Enum.ToObject(messageTypeEnum, 0);
        //    return CreateEvent(EventTrigger.AfterSendMessage,
        //        0,
        //        messageType,
        //        -1,
        //        new AttributeList());
        //}

        //////////////////////////////////////////////////////////////////////////////////////////////////////
        ///// \fn public static Attribute CreateEvent(EventTrigger eventTrigger, int eventRound, dynamic eventMessageType, int eventMessageOtherEnd, AttributeList methods)
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
        ///// \param methods               (AttributeList) - The methods.
        /////
        ///// \return The new event.
        //////////////////////////////////////////////////////////////////////////////////////////////////////

        //public static Attribute CreateEvent(EventTrigger eventTrigger,
        //    int eventRound,
        //    dynamic eventMessageType,
        //    int eventMessageOtherEnd,
        //    AttributeList methods)
        //{           
        //    Attribute eventAttribute = CreateEvent(eventTrigger, eventRound, eventMessageType, eventMessageOtherEnd);
        //    eventAttribute.Value.Add(EventParts.EventAction, new Attribute { Value = methods, ElementWindowPrmsMethod= MethodsListPrms });
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
        /////      -  Process Internal events.  
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
        //    base.ProcessEvents(trigger, message, (AttributeList)process.op[bp.opk.InternalEvents]);
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
        //    AttributeList methods = (AttributeList)eventAction;
        //    for (int idx = 0; idx < methods.Count; idx ++)
        //    {
        //        TypesUtility.InvokeMethod(process, methods[idx], new List<object>(), 1, false);
        //    }            
        //}
        //#endregion
        //#region /// \name ElementWindow support

        //////////////////////////////////////////////////////////////////////////////////////////////////////
        ///// \fn public static ElementWindowPrms MethodsListPrms(Attribute attribute, dynamic key, NetworkElement mainNetworkElement, NetworkElement.ElementDictionaries mainDictionary, NetworkElement.ElementDictionaries dictionary, InputWindows inputWindow, bool windowEditable)
        /////
        ///// \brief Methods list prms.
        /////
        ///// \par Description.
        /////      -  This method sets the ElementWindowprms for the method list  
        /////      -  The role of this method is to replace the regular method for adding attribute with  
        /////         the method AddMethod  
        /////         
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

        //public static ElementWindowPrms MethodsListPrms(Attribute attribute,
        //    dynamic key,
        //    NetworkElement mainNetworkElement,
        //    NetworkElement.ElementDictionaries mainDictionary,
        //    NetworkElement.ElementDictionaries dictionary,
        //    InputWindows inputWindow,
        //    bool windowEditable)
        //{

        //    ElementWindowPrms elementWindowPrms = new ElementWindowPrms();
        //    elementWindowPrms.newValueControlPrms.inputFieldType = InputFieldsType.AddRemovePanel;
        //    elementWindowPrms.newValueControlPrms.add_button_click = AddMethod;
        //    return elementWindowPrms;
        //}

        //////////////////////////////////////////////////////////////////////////////////////////////////////
        ///// \fn public static Attribute AddMethod()
        /////
        ///// \brief Adds method.
        /////
        ///// \par Description.
        /////      This method is used in the GUI to add a new attribute holding the default method name
        /////      when the Add button of the Methods List is pressed
        /////
        ///// \par Algorithm.
        /////
        ///// \par Usage Notes.
        /////
        ///// \author Ilanh
        ///// \date 16/01/2018
        /////
        ///// \return An Attribute.
        //////////////////////////////////////////////////////////////////////////////////////////////////////
        //public static Attribute AddMethod()
        //{
        //    return new Attribute { Value = "DefaultInternalEvent", ElementWindowPrmsMethod = MethodComboBoxPrms };
        //}

        //////////////////////////////////////////////////////////////////////////////////////////////////////
        ///// \fn public static ElementWindowPrms MethodComboBoxPrms(Attribute attribute, dynamic key, NetworkElement mainNetworkElement, NetworkElement.ElementDictionaries mainDictionary, NetworkElement.ElementDictionaries dictionary, InputWindows inputWindow, bool windowEditable)
        /////
        ///// \brief Method combo box prms.
        /////
        ///// \par Description.
        /////      This method is used in the GUI to define the combo box of the possible action methods
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

        //public static ElementWindowPrms MethodComboBoxPrms(Attribute attribute,
        //    dynamic key,
        //    NetworkElement mainNetworkElement,
        //    NetworkElement.ElementDictionaries mainDictionary,
        //    NetworkElement.ElementDictionaries dictionary,
        //    InputWindows inputWindow,
        //    bool windowEditable)
        //{
        //    ElementWindowPrms elementWindowPrms = new ElementWindowPrms();
        //    elementWindowPrms.newValueControlPrms.inputFieldType = InputFieldsType.ComboBox;
        //    string[] options = TypesUtility.GetAllInternalEventMethods().ToArray();
        //    elementWindowPrms.newValueControlPrms.options = options;
        //    elementWindowPrms.newValueControlPrms.Value = attribute.Value;
        //    return elementWindowPrms;
        //}
        //#endregion
//    }
//}
