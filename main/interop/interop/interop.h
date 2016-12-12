// interop.h

#include <messenger.h>
#include "types.h"
#include "callbacks.h"
#include "observers.h"
#include "settings.h"
#pragma once

typedef void(*LoginCallback)(messenger::operation_result::Type);
typedef void(*RequestUsersCallback)(messenger::operation_result::Type, const messenger::UserList&);

class CallbackProxy : public messenger::ILoginCallback, public messenger::IRequestUsersCallback
{
public:
	LoginCallback loginCallback = NULL;
	RequestUsersCallback requestUsersCallback = NULL;
private:
	void OnOperationResult(messenger::operation_result::Type result) override {
		if (loginCallback != NULL) loginCallback(result);
		loginCallback = NULL;
	}

	void OnOperationResult(messenger::operation_result::Type result, const messenger::UserList& users) override {
		if (requestUsersCallback != NULL) requestUsersCallback(result, users);
		requestUsersCallback = NULL;
	}

};



static std::shared_ptr<messenger::IMessenger> g_messenger;
static CallbackProxy g_callbackProxy;
static LoginCallback g_loginCallback;
static RequestUsersCallback g_usersCallback;

extern "C" {
	void __declspec(dllexport) Init() {
		messenger::MessengerSettings settings;
		settings.serverPort = 5222;
		settings.serverUrl = "localhost";
		g_messenger = messenger::GetMessengerInstance(settings);

	}

	void __declspec(dllexport) GetStruct(messenger::SecurityPolicy* securityPolicy);
	
	

	void __declspec(dllexport) Login(LoginCallback loginCallback) {
		std::string loginStr("asd@localhost");
		std::string passwordStr("");
		messenger::SecurityPolicy securityPolicy;
		g_callbackProxy.loginCallback = g_loginCallback;
		g_messenger->Login(loginStr, passwordStr, securityPolicy, &g_callbackProxy);
	}
	
	void __declspec(dllexport) RequestActiveUsers(RequestUsersCallback requestUsersCallback) {
		g_callbackProxy.requestUsersCallback = g_usersCallback;
		g_messenger->RequestActiveUsers(&g_callbackProxy);
	}

	void __declspec(dllexport) Disconnect() {
		g_messenger->Disconnect();
	}
			}

