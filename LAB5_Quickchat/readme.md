## Laboratory Work 4 - Reverse engineering a network protocol
### Objectives:
Understand and document the protocol used by **QuickChat** - a chat program for local area networks.
- get used to reverse engineering
- learn to appreciate the documentation, when you have it
- learn to document your activities

### Requirements:
The QuickChat protocol (QCP) must be reverse-engineered first. The result of reverse-engineering must be a document that shows which messages are sent to the network when an event is triggered.  
The document must cover the following minimal set of events/actions:
- sending a public message
- sending a private message
- changing the chat topic
- changing a nickname
- changing the status
- creating, joining, leaving a channel

Example:
>! change nickname: "05 %s-old_nickname% %s-new_nickname%"  

- The document must also contain a high-level description of QCP that explains the logic of the client and the order in which messages are sent in different contexts.

#### Extended requirments
Build a minimalistic client which provides these features:
- *spoofing* - send a private or a public message on someone else's behalf  - *noPrivacy* - read all private messages that were sent to other users

**Notes** 
- The requirements state that you need to reverse engineer and document the protocol, there is no need to make a program that implements that protocol;
- However, it will be easier for you if you make a set of primitive functions that can perform each action - this will allow you to test your guesses about the protocol's modus operandi;
- If you build such a program, it does not have to be one with a GUI, a command line application will do.

### The solution:
**QuickChat** is a very simple chat application which can be used in a local network for text communication. It is using IPV4 protocol at the network layer and UDP at the transport layer.  There is a special definition for the *IP* broadcast adress 255.255.255.255. This is the broadcast address of the zero network, which stands for local network.  

The **QuickChat** client listens at this ip adress at port **8167**. The port value can be found somewhere in **QuickChat's** settings.   
When we try to create a socket and bound it to this port simultaneously with **QuickChat** we get an error, adress is already in use. This means that reuseAddr socket option isn't enabled.

####Message format:
QC sends an UDP datagram when a event is triggered. In the following section there is information about the format of every message.

*(w): by default every string ends with a null, (w) means without null. If (w) is absend it means that the string ends with a null.  

*+: empty space, i used it as a separator for a more human readable form.*
 
- **Change Nickname**: 
      3(w) + Current Nickname + Next Nickname + 0(w)
- **Change Status**:
      D(w) + nickname + status(w)

 where *status*: 20 - away , 10 - dnd , 00 - normal , 30 - offline

- **Leave Channel**: 
      5(w) + nickname + #channel name + 0(w)

- **Public Message**:
      2(w) + #Channel Name + nickname + message

- **New Topic**: 
      B(w) + topic name(w) + (nickname(w)) 

- **Private message**: 
      6(w) + nickname1 + nickname2 + message

- **Open channel**:       
      N(w) + username  
      O(w) + username + #channel1 name#channel2 name#


- **Join channel**: 
      4(w) + username + #channel name + 00(w)
      L(w) + username + #channel name
      K(w) + username + #channel name + username + 1(w)

### Short program descroption
For this laboratory work i made 2 programs. One is used to send udp packets that perform the operations mentioned in the requirments section and another is used for capturing the private messages in real time. The first program was written in c++ (visual studio 2012) and the second one is in C#. The source code can be downloaded using the above links.

###Conclusion
In this laboratory work we learned how to reverse engineer a network protocol. By analysing the packets send by this program in **WireShark**, we were able to create a program similar to **QuickChat** that can interct with it. The problem with QC is that it doesnt use any form of encryption, so in this way all the messages can be sniffed very easy. 
 



