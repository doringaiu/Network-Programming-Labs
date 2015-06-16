#define WIN32_LEAN_AND_MEAN
#define _WINSOCK_DEPRECATED_NO_WARNINGS
#include <windows.h>
#include <winsock2.h>
#include <ws2tcpip.h>
#include <stdlib.h>
#include <stdio.h>
#include <string>
#include <cstdlib>
#include <iostream>
#include <thread>
#include <future>
#pragma comment (lib, "Ws2_32.lib")
#define SCK_VERSION2 0x0202
#define menuInfo "\n \t1: connect to server \n \t2: receive packets \n \t3: input commands\n \t9: exit \n\n \t>:"

char* inputString();
int changeNickname(SOCKET socket1, char *currentNick, char *nextNick);
int changeStatus(SOCKET socket1, char *nickname, char *status);
int leaveChannel(SOCKET socket1, char *nickname, char *channelName);
int sendPublicMessage(SOCKET socket1, char *channelName, char *nickname, char *message);
int newTopic(SOCKET socket1, char *nickname, char *topicName);
int sendPrivateMessage(SOCKET socket, char *nickname1, char *nickname2, char *message);
int joinChannel(SOCKET socket, char *userName, char *channelName);
int openChannel(SOCKET socket, char *userName, char *channelName);
void interceptPrivateMessages(char *buffer);