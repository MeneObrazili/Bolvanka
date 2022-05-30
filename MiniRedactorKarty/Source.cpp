#include<Windows.h>
#include<vector>
#include<iostream>
#pragma warning(disable:4996)
wchar_t						szClassName[] = L"Classek_proektika";
wchar_t						szWinName[256] = L"Enter_map_sizes";
HINSTANCE					hInstance = 0;
HWND						hHWND = 0;
WNDCLASSEXW					WCExW;
DWORD						dwStyle = WS_OVERLAPPEDWINDOW & ~WS_THICKFRAME & ~WS_MAXIMIZEBOX;
DWORD						dwStyleEx =				0;
RECT						WNDRECT =				{0, 0, 800, 600};
MSG							WNDMESSAGE;

byte started;

HWND hWndVvodax;

HWND hWndVvoday;

HWND hWndStart;

char vvodx[256];
char vvody[256];

struct nachalo {
	DWORD UniKey;
	DWORD SizeX;
	DWORD SizeY;
	DWORD pToNextStruct;
};

struct Next {
	DWORD kolvo;

};

typedef struct boec {
	DWORD dwTip;
	DWORD bNash;
	LONG x;
	LONG y;
} *lpboec;
byte bZapisyBoyca = 0;
int x, y, *pointer, dwVvod; std::vector<boec> boysyi;

DWORD FindBoyca(int x, int y) {
	for(int i = 0; i < boysyi.size(); i++) {
		if(boysyi[i].x == x && boysyi[i].y == y)
			return i;
	}
	return 0xffffffff;
}

RECT mainrect = {0, 0, 800, 600};

HBRUSH hBrushblya = CreateSolidBrush(0x00ffffff);
#define ifn(huita) if(!(huita))
LRESULT  WINAPI  MsgProc(HWND hWnd, UINT msg, WPARAM wParam, LPARAM lParam);
int WINAPI WinMain(HINSTANCE hInst, HINSTANCE hPrevInstance, LPSTR lpCmdLine, int nCmdShow){

	hInstance = hInst;
	memset(&WCExW, 0, sizeof(WCExW));
	WCExW.cbSize = sizeof(WCExW);
	WCExW.lpfnWndProc = MsgProc;
	WCExW.hInstance = hInstance;
	WCExW.lpszClassName = szClassName;
	RegisterClassExW(&WCExW);
	hHWND = CreateWindowExW(dwStyleEx, szClassName, szWinName, dwStyle, WNDRECT.left, WNDRECT.top, WNDRECT.right, WNDRECT.bottom, 0, 0, hInstance, 0);
	if (!hHWND)
	{
		MessageBoxW(0, L"Cannot_create_hwnd", L"Trouble_with_wnd", 0);
		return 0;
	}
	ShowWindow(hHWND, SW_SHOWNORMAL);
	UpdateWindow(hHWND);

	hWndVvodax = CreateWindowExA(0, "edit", 0, WS_CHILD | WS_VISIBLE | WS_BORDER | ES_LEFT, 0, 20, 100, 20, hHWND, (HMENU)1, hInst, 0);

	hWndVvoday = CreateWindowExA(0, "edit", 0, WS_CHILD | WS_VISIBLE | WS_BORDER | ES_LEFT, 0, 60, 100, 20, hHWND, (HMENU)2, hInst, 0);

	hWndStart = CreateWindowExA(0, "button", "Старт редактирования", WS_CHILD | WS_VISIBLE | WS_BORDER,0, 80, 300, 20, hHWND, (HMENU)3, hInst, 0);

	while (WNDMESSAGE.message != WM_QUIT) {
		while (GetMessageW(&WNDMESSAGE, 0, 0, 0)){
			TranslateMessage(&WNDMESSAGE);
			DispatchMessageW(&WNDMESSAGE);
		}
	}
	UnregisterClassW(szClassName, hInstance);
	ExitProcess(0);
}


LRESULT WINAPI MsgProcBoec(HWND hWnd, UINT msg, WPARAM wParam, LPARAM lParam) {
	LRESULT output = 0;
	switch(msg) {
	case WM_PAINT: 
	{
		PAINTSTRUCT structura; RECT RTip = {0, 0, 300, 20}, RNash = {0, 40, 300, 60}, RLeft = {0, 80, 300, 100}, RTop = {0, 120, 300, 140};
		HDC hdc = BeginPaint(hWnd, &structura);
		FillRect(hdc, &structura.rcPaint, hBrushblya);
		DrawTextA(hdc, "Тип бойца от 0 до 3", 19, &RTip, 0);
		DrawTextA(hdc, "Наш(1) боец или нет(0)", 22, &RNash, 0);
		DrawTextA(hdc, "Координата бойца по х", 21, &RLeft, 0);
		DrawTextA(hdc, "Координыта бойца по у", 21, &RTop, 0);
		EndPaint(hWnd, &structura);
		break;
	}
	case WM_COMMAND:
	{
		SendMessageA(hHWND, WM_COMMAND, wParam, 0);
		break;
	}
	default:
		output = DefWindowProcW(hWnd, msg, wParam, lParam);
		break;
	}
	return output;
}

HWND hBoecWnd;

HWND hTipWnd,
hNashWnd,
hLeftWnd,
hTopWnd,
hButtonWnd;



LRESULT  WINAPI  MsgProc(HWND hWnd,UINT msg,WPARAM wParam,LPARAM lParam){
	LRESULT output = 0;
	if(bZapisyBoyca) {
		ifn(msg == WM_COMMAND && wParam == 4)
			return DefWindowProcW(hWnd, msg, wParam, lParam);
		DestroyWindow(hBoecWnd); bZapisyBoyca = 0;
		return 0;
	}
	if(!started)
		goto notstarted;
	switch (msg){
	case WM_KEYDOWN:
		if(wParam >= 0x30 && wParam <= 0x39)
			dwVvod = wParam - 0x30;
		if(wParam == 'Q') {
			WNDCLASSEXW wndclass;
			memset(&wndclass, 0, sizeof(wndclass));
			wndclass.cbSize = sizeof(wndclass);
			wndclass.lpfnWndProc = MsgProcBoec;
			wndclass.lpszClassName = L"nigra";
			RegisterClassExW(&wndclass);
			DWORD loh = GetLastError(); std::cout << loh;
			hBoecWnd = CreateWindowExW(0, L"nigra", 0, WS_CHILD | WS_VISIBLE | WS_BORDER, 0, 0, 400, 180, hHWND, 0, hInstance, 0);

			hTipWnd = CreateWindowExA(0, "edit", 0, WS_CHILD | WS_VISIBLE | WS_BORDER, 0, 20, 400, 20, hBoecWnd, 0, hInstance, 0);
			hNashWnd = CreateWindowExA(0, "edit", 0, WS_CHILD | WS_VISIBLE | WS_BORDER, 0, 60, 400, 20, hBoecWnd, 0, hInstance, 0);
			hLeftWnd = CreateWindowExA(0, "edit", 0, WS_CHILD | WS_VISIBLE | WS_BORDER, 0, 100, 400, 20, hBoecWnd, 0, hInstance, 0);
			hTopWnd = CreateWindowExA(0, "edit", 0, WS_CHILD | WS_VISIBLE | WS_BORDER, 0, 140, 400, 20, hBoecWnd, 0, hInstance, 0);
			hButtonWnd = CreateWindowExA(0, "button", "Готово", WS_CHILD | WS_VISIBLE | WS_BORDER, 0, 160, 400, 20, hBoecWnd, (HMENU)4, hInstance, 0);
			bZapisyBoyca = 1;
		}
		break;
	case WM_KEYUP:
		break;
	case WM_MOUSEWHEEL:
		break;
	case WM_MOUSEMOVE:
		break;
	case WM_LBUTTONDOWN:
	{
		int dwXPOS = lParam & 0xffff, dwYPOS = lParam >> 16;
		pointer[(dwXPOS / 40) + (dwYPOS / 40) * x] = dwVvod;
		SendMessageA(hWnd, WM_PAINT, 0, 0);
		InvalidateRect(hHWND, &mainrect, 1);
		break;
	}
	case WM_LBUTTONUP:
		break;
	case WM_PAINT: 
	{
		PAINTSTRUCT structura; RECT rectblya1 = {0, 0, 300, 20}; RECT rectblya2 = {0, 40, 300, 60};
		HDC hdc = BeginPaint(hWnd, &structura);
		sprintf(vvodx, "intuha=%d", x); sprintf(vvody, "intuha=%d", y);
		FillRect(hdc, &structura.rcPaint, hBrushblya);
		for(int i = 0; i < x * y; i++) {
			RECT recta = {i % x * 40, i / x * 40, i % x * 40 + 40, i / x * 40 + 40};
			Rectangle(hdc, recta.left, recta.top, recta.right, recta.bottom);
			
			sprintf(vvodx, "%d", pointer[i]);
			DrawTextA(hdc, vvodx, 2, &recta, DT_CENTER | DT_VCENTER | DT_SINGLELINE);
		}
		EndPaint(hWnd, &structura);
		RECT  rectas;
		break;
	}
	case WM_CLOSE:
	{
		HANDLE hFile = CreateFileW(L"output.bin", GENERIC_WRITE, 0, 0, CREATE_NEW, 0, 0);
		LPVOID lpPointer = HeapAlloc(GetProcessHeap(), 0, 16 + x*y);
		((int *)lpPointer)[0] = 0x00455541;
		((int *)lpPointer)[1] = x;
		((int *)lpPointer)[2] = y;
		((int *)lpPointer)[3] = 0x10;
		for(int i = 0; i < x * y; i++) {
			((char*)lpPointer)[16 + i] = (unsigned char)pointer[i];
		}
		DWORD buffer;
		WriteFile(hFile, lpPointer, 16 + x * y, &buffer, 0);
		CloseHandle(hFile);
	}
		SendMessageW(hWnd, WM_DESTROY, 0, 0);
		break;
	case WM_DESTROY:
		PostQuitMessage(0);
		break;
	case WM_COMMAND:
		break;
	default:
		output = DefWindowProcW(hWnd, msg, wParam, lParam);
		break;
	}
	return output;
notstarted:
	switch(msg) {
	case WM_PAINT: 
	{
		PAINTSTRUCT structura; RECT rectblya1 = {0, 0, 300, 20}; RECT rectblya2 = {0, 40, 300, 60};
		HDC hdc = BeginPaint(hWnd, &structura);
		FillRect(hdc, &structura.rcPaint, hBrushblya);
		DrawTextA(hdc, "Ширина Карты", 12, &rectblya1, 0);
		DrawTextA(hdc, "Высота Карты", 12, &rectblya2, 0);
		EndPaint(hWnd, &structura);
		break;
	}
	case WM_CLOSE:
		SendMessageW(hWnd, WM_DESTROY, 0, 0);
		break;
	case WM_DESTROY:
		PostQuitMessage(0);
		break;
	case WM_COMMAND:
		switch(wParam) {
		case 3:
		{
			RECT rectt;
			GetWindowTextA(hWndVvodax, vvodx, 256);
			GetWindowTextA(hWndVvoday, vvody, 256);
			x = atoi(vvodx);
			y = atoi(vvody);
			pointer = (int*)HeapAlloc(GetProcessHeap(), HEAP_ZERO_MEMORY, (x * y) << 2);
			DestroyWindow(hWndVvodax);
			DestroyWindow(hWndVvoday);
			started = !started;
			RECT rect = {0,0,800,600};
			DestroyWindow(hWndStart);
			SetWindowTextW(hHWND, L"0-9 - тип тайла. q - без войск, w - наши бойцы, e - вражеские бойцы");
			InvalidateRect(hHWND, &rect, 1);
		}
		break;
		}
	default:
		output = DefWindowProcW(hWnd, msg, wParam, lParam);
		break;
	}
	return output;
}