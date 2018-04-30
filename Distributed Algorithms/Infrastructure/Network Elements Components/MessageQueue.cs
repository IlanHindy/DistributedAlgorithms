using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DistributedAlgorithms.Algorithms.Base.Base;

namespace DistributedAlgorithms
{
    public enum MessageQOperation { AddMessage, RetrieveFirstMessage, RemoveFirstMessage, EmptyQueue, Init, BlockMessageProcessing, ReleaseMessageProcessing }
    public class MessageQ:AttributeList
    { 
        private BaseProcess process;

        private ManualResetEvent MessageQEvent = new ManualResetEvent(false);


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// \brief The message queue lock. This lock is used to lock the message queue
        ///        The lock is needed because many (all the reading threads) are accessing the same queue.
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        private object MessageQLock = new object();

        public MessageQ():base()
        {

        }

        public override void SetMembers(BaseNetwork network, NetworkElement element, Permissions permissions, IValueHolder parent, bool recursive = true)
        {
            base.SetMembers(network, element, permissions, parent);
            process = (BaseProcess)element;
        }
        public void MessageQHandling(ref BaseMessage message, MessageQOperation operation, int additionIndex = -1)
        {
            switch (operation)
            {
                case MessageQOperation.Init: InitMessageQ(); return;
                case MessageQOperation.AddMessage: AddMessageToQueue(message, additionIndex); return;
                case MessageQOperation.RetrieveFirstMessage: RetrieveFirstMessageFromQueue(ref message); return;
                case MessageQOperation.RemoveFirstMessage: RemoveFirstMessageFromQueue(); return;
                case MessageQOperation.EmptyQueue: EmptyMessageQ(); return;
                case MessageQOperation.BlockMessageProcessing: BlockMessageProcessing(); return;
                case MessageQOperation.ReleaseMessageProcessing: ReleaseMessageProcessing(); return;
                default: return;
            }
        }

        private void InitMessageQ()
        {
            MessageQEvent.Reset();
        }

        private void SetMessagesPositionInQ()
        {
            for (int idx = 0; idx < Count; idx++)
            {
                this[idx].SetField(bm.ork.PositionInProcessQ, idx);
            }
        }

        private void AddMessageToQueue(BaseMessage message, int additionIndex)
        {
            // This method is used by all the receive threads so it has to be protected by a lock
            lock (MessageQLock)
            {
                if (additionIndex == -1)
                {
                    Add(new Attribute { Value = message });
                }
                else
                {
                    Insert(additionIndex, new Attribute { Value = message });
                }
                SetMessagesPositionInQ();
                MessageQEvent.Set();
            }
        }

        private void RetrieveFirstMessageFromQueue(ref BaseMessage message)
        {
            // If the Q is empty the main thread process will wait on this event until a message
            // will arrive
            MessageQEvent.WaitOne();

            // This is a virtual method that make it possible to arrange the messages in the
            // message Q not according to the order they arrived
            process.ArrangeMessageQ(this);

            // This is a message that make it possible to hold a processing of the first message in the Q
            // If a hold is done an empty message will return that will cause the ReceiveLoopEnvelope
            // To skip the call to ReceiveHandeling (start a new iteration)
            if (process.MessageProcessingCondition(this[0]))
            {
                message = this[0];
            }
            else
            {
                message = new BaseMessage(network);
            }
            SetMessagesPositionInQ();
        }

        private void RemoveFirstMessageFromQueue()
        {
            Logger.Log(Logger.LogMode.MainLogAndMessageTrace, process.GetProcessDefaultName(), "", "In RemoveFirstMessageFromQueue  ", "", "RunningWindow");
            if (Count > 0)
            {
                RemoveAt(0);
                SetMessagesPositionInQ();
                if (Count == 0)
                {
                    MessageQEvent.Reset();
                }
            }
        }

        private void EmptyMessageQ()
        {
            Logger.Log(Logger.LogMode.MainLogAndMessageTrace, process.GetProcessDefaultName(), "", "In EmptyMessageQ  ", "", "RunningWindow");
            while (Count > 0)
            {
                RemoveAt(0);
            }
            MessageQEvent.Reset();
        }

        private void BlockMessageProcessing()
        {
            MessageQEvent.Reset();
        }

        private void ReleaseMessageProcessing()
        {
            if (Count == 0)
            {
                MessageQEvent.Reset();
            }
            else
            {
                MessageQEvent.Set();
            }
        }
    }
}
