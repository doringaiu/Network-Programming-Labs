#include "Header.h"
using namespace std;

char* inputString()
{
	char* someChar;
	string someString;
	cin >> someString;
	int tempLength = strlen(someString.c_str()) + 1;
	someChar = new char[tempLength];
	memcpy(someChar, someString.c_str(), tempLength);
	return someChar;
}

int changeNickname(SOCKET socket1, char *currentNick, char *nextNick)
{
	int currentNickSize = strlen(currentNick)+1;
	int nextNickSize = strlen(nextNick)+1;
	char *sendData = new char[currentNickSize + nextNickSize + 2];
	memcpy(sendData, "3", 1);
	memcpy(sendData + 1, currentNick, currentNickSize);
	memcpy(sendData + currentNickSize + 1, nextNick, nextNickSize);
	memcpy(sendData + currentNickSize + nextNickSize + 1, "0", 1);

	int iResult = send(socket1, sendData, currentNickSize + nextNickSize + 2, NULL);
	if (iResult == SOCKET_ERROR)
	{
		cout << "\n \t error \n \t";
	}

	return iResult;
}

int changeStatus(SOCKET socket1, char *nickname, char *status)
{
	int nicknameSize = strlen(nickname) + 1;
	char *sendData = new char[nicknameSize + 3];
	memcpy(sendData, "D", 1);
	memcpy(sendData + 1, nickname, nicknameSize);
	memcpy(sendData + nicknameSize + 1, status, 2);

	int iResult = send(socket1, sendData, nicknameSize + 3, NULL);
	if (iResult == SOCKET_ERROR)
	{
		cout << "\n \t error \n \t";
	}

	return iResult;

}

int leaveChannel(SOCKET socket1, char *nickname, char *channelName)
{
	int nicknameSize = strlen(nickname) + 1;
	int channelSize = strlen(channelName) + 1;
	char *sendData = new char[nicknameSize + 3];
	memcpy(sendData, "5", 1);
	memcpy(sendData + 1, nickname, nicknameSize);
	memcpy(sendData + nicknameSize + 1, channelName, channelSize);
	memcpy(sendData + nicknameSize + 1 + channelSize, "0", 1);

	int iResult = send(socket1, sendData, nicknameSize + channelSize + 2, NULL);
	if (iResult == SOCKET_ERROR)
	{
		cout << "\n \t error \n \t";
	}

	return iResult;
}

int sendPublicMessage(SOCKET socket1, char *channelName, char *nickname, char *message)
{
	int nicknameSize = strlen(nickname) + 1;
	int channelSize = strlen(channelName) + 1;
	int messageSize = strlen(message) + 1;
	int maxSize = messageSize + channelSize + nicknameSize + 1;
	char *sendData = new char[maxSize];
	memcpy(sendData, "2", 1);
	memcpy(sendData + 1, channelName, channelSize);
	memcpy(sendData + channelSize + 1, nickname, nicknameSize);
	memcpy(sendData + maxSize - messageSize,message, messageSize);

	int iResult = send(socket1, sendData, maxSize, NULL);
	if (iResult == SOCKET_ERROR)
	{
		cout << "\n \t error \n \t";
	}

	return iResult;
}

int newTopic(SOCKET socket1, char *nickname, char *topicName)
{
	int nicknameSize = strlen(nickname);
	int topicSize = strlen(topicName);
	int maxSize = nicknameSize + topicSize + 5;
	char *sendData = new char[maxSize];
	memcpy(sendData, "B", 1);
	memcpy(sendData + 1, topicName, topicSize);
	memcpy(sendData + 1 + topicSize, " (", 2);
	memcpy(sendData + 3 + topicSize, nickname, nicknameSize);
	memcpy(sendData + maxSize - 2, ")",1);
	sendData[maxSize - 1] = NULL;

	int iResult = send(socket1, sendData, maxSize, NULL);
	if (iResult == SOCKET_ERROR)
	{
		cout << "\n \t error \n \t";
	}

	return iResult;

}

int sendPrivateMessage(SOCKET socket, char *nickname1, char *nickname2, char *message)
{
	int sizes[3] = { strlen(nickname1) + 1, strlen(nickname2) + 1, strlen(message) + 1 };
	int maxSize = sizes[0] + sizes[1] + sizes[2] + 1;
	char *sendData = new char[maxSize];
	memcpy(sendData, "6", 1);
	memcpy(sendData + 1, nickname1, sizes[0]);
	memcpy(sendData + 1 + sizes[0], nickname2, sizes[1]);
	memcpy(sendData + maxSize - sizes[2], message, sizes[2]);

	int iResult = send(socket, sendData, maxSize, NULL);
	if (iResult == SOCKET_ERROR)
	{
		cout << "\n \t error \n \t";
	}

	return iResult;
}

int joinChannel(SOCKET socket, char *userName, char *channelName)
{
	int sizes[2] = { strlen(userName) + 1, strlen(channelName) + 1 };
	int maxSize = sizes[0] + sizes[1] + 3;
	char *sendData = new char[maxSize];
	memcpy(sendData, "4", 1);
	memcpy(sendData + 1, userName, sizes[0]);
	memcpy(sendData + 1 + sizes[0], channelName, sizes[1]);
	memcpy(sendData + maxSize - 2, "00", 2);

	char *sendData2 = new char[maxSize - 2];
	memcpy(sendData2, sendData, maxSize - 2);
	memcpy(sendData2, "L", 1);

	char *sendData3 = new char[maxSize - 1 + sizes[0]];
	memcpy(sendData3, sendData2, maxSize - 2);
	memcpy(sendData3 + maxSize - 2, userName, sizes[0]);
	memcpy(sendData3 + maxSize - 2 + sizes[0], "1", 1);
	memcpy(sendData3, "K", 1);

	send(socket, sendData, maxSize, NULL);
	send(socket, sendData2, maxSize - 2, NULL);
	int iResult = send(socket, sendData3, maxSize - 1 + sizes[0], NULL);
	if (iResult == SOCKET_ERROR)
	{
		cout << "\n \t error \n \t";
	}

	return iResult;
}

int openChannel(SOCKET socket, char *userName, char *channelName)
{
	int userNameLength = strlen(userName) + 1;
	int channelNameLengths = strlen(channelName) + 1;
	char *sendData1 = new char[userNameLength + 1];
	memcpy(sendData1, "N", 1);
	memcpy(sendData1 + 1, userName, userNameLength);
	int maxSize = userNameLength + 1 + channelNameLengths;
	char *sendData2 = new char[maxSize];
	memcpy(sendData2, sendData1, userNameLength + 1);
	memcpy(sendData2, "O", 1);
	memcpy(sendData2 + 1 + userNameLength, channelName, channelNameLengths);

	send(socket, sendData1, userNameLength + 1, NULL);
	int iResult = send(socket, sendData2, maxSize, NULL);
	if (iResult == SOCKET_ERROR)
	{
		cout << "\n \t error \n \t";
	}

	return iResult;
}
//
//void interceptPrivateMessages(char *buffer)
//{
//	int iResult;
//
//	do {
//		//iResult = recvfrom(socket , buffer, strlen(buffer) + 1, 0, (SOCKADDR *) & SenderAddr, &SenderAddrSize); );
//		if (iResult > 0)
//			printf("Bytes received: %d\n", iResult);
//		else if (iResult == 0)
//			printf("Connection closed\n");
//		else
//			printf("recv failed with error: %d\n", WSAGetLastError());
//
//	} while (iResult > 0);
//
//}