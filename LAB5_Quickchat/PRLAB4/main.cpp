#include "Header.h"

using namespace std;

int main()
{
	WSADATA wsa;
	SOCKADDR_IN serverInfo;
	SOCKET socket1;
	int iResult = WSAStartup(MAKEWORD(2, 1), &wsa);
	char *receivedBuffer = new char[23];
	char *receivedMessage = NULL;
	char keyboardSelect = NULL;
	char *recvbuf = new char[512];
	

	cout << menuInfo;
	while (keyboardSelect != '9')
	{
		keyboardSelect = getchar();

		switch (keyboardSelect)
		{

		case '1': // connect to server
			system("cls");

			socket1 = socket(AF_INET, SOCK_DGRAM, IPPROTO_UDP);
			serverInfo.sin_addr.s_addr = inet_addr("255.255.255.255");
			serverInfo.sin_port = htons(8167);
			serverInfo.sin_family = AF_INET;

			iResult = connect(socket1, (SOCKADDR*)&serverInfo, sizeof(serverInfo));
			if (iResult == SOCKET_ERROR)
			{
				closesocket(socket1);
				socket1 = INVALID_SOCKET;
				cout << "\n \t socket error \n \t" << endl;
			}

			if (iResult == 0)
			{
				cout << "Success \n";
			}

			cout << menuInfo;
			break;

		case '2': // receive data
			system("cls");
			iResult = recv(socket1, receivedBuffer, sizeof(receivedBuffer), 0);

			if (receivedBuffer != NULL)
			{
				cout << "\n \t Message from the server: \n \t" << receivedBuffer << endl;
			}
			else
			{
				cout << "\n \t error \n \t";
			}

			cout << menuInfo;
			break;

		case '3': // send data
			system("cls");
			int inputCommand;
			cout << "\n\t1 - send public message\n\t2 - send private message\n\t";
			cout << "3 - change chat topic\n\t4 - change nickname\n\t5 - change status \n\t";
			cout << "6 - create channel\n\t7 - join channel\n\t8 - leave channel\n\n\t>:";
			cin >> inputCommand;
			switch (inputCommand)
			{
			case 1: // public message
			{
						cout << "\n\tType username  :";
						char *userName = inputString();
						cout << "\n\tType channel name  :";
						char *channelName = inputString();
						cout << "\n\tType the message  :";
						char *message = inputString();
						iResult = sendPublicMessage(socket1, channelName, userName, message);
			}
				break;

			case 2: // private message
			{
						cout << "\n\tType src username  :";
						char *userName1 = inputString();
						cout << "\n\tType dst username  :";
						char *userName2 = inputString();
						cout << "\n\tType the message  :";
						char *message = inputString();
						iResult = sendPrivateMessage(socket1, userName1, userName2, message);
			}
				break;

			case 3: // change chat topic
			{
						cout << "\n\tType username  :";
						char *userName = inputString();
						cout << "\n\tType topic name  :";
						char *topicName = inputString();
						iResult = newTopic(socket1, userName, topicName);

			}
				break;
			case 4: // change nickname
			{
						cout << "\n\tType username  :";
						char *userName = inputString();
						cout << "\n\tType username  :";
						char *nextUserName = inputString();
						iResult = changeNickname(socket1, userName, nextUserName);
			}
				break;

			case 5: // change status
			{
						cout << "\n\tType username  :";
						char *userName = inputString();
						cout << "\n \tstatus change: 20 - away , 10 - dnd , 00 - normal , 30 - offline \n\t  :";
						char *selection = inputString();
						iResult = changeStatus(socket1, userName, selection);
			}
				break;

			case 6: // create channel
			{
						cout << "\n\tType src username  :";
						char *userName = inputString();
						cout << "\n \tchannel names in the form: #Ch1#Ch2#...#ChN# \n\t  :";
						char *channelName = inputString();
						iResult = openChannel(socket1, userName, channelName);
			}	
				break;

			case 7: // join channel
			{
						cout << "\n\tType src username  :";
						char *userName = inputString();
						cout << "\n \tchannel name \n\t  :";
						char *channelName = inputString();
						iResult = joinChannel(socket1, userName, channelName);
			}
				break;
			case 8: // leave channel
			{
						cout << "\n\tType username  :";
						char *userName = inputString();
						cout << "\n\tType channel name  :";
						char *channelName = inputString();
						iResult = leaveChannel(socket1, userName, channelName);
			}
				break;

			case 0:
			{	  
					  thread t(bind(interceptPrivateMessages, recvbuf));
					  t.detach();		  
			}
				break;
			}	
			
			cout << menuInfo;
			break;
		}
	}

	closesocket(socket1);
	WSACleanup();
	system("cls");
	system("pause");
	return 0;
}
