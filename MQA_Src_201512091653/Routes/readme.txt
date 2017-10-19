若要使用此範本搭配 Windows Azure  驗證，請參閱  "http://go.microsoft.com/fwlink/?LinkID=267940" http://go.microsoft.com/fwlink/?LinkID=267940。

否則，若要使用此範本搭配 Windows  驗證，請參閱下列指示：

在 IIS Express 中裝載：
1. 按一下 [方案總管] 中的專案，選取專案。
2. 如果 [屬性] 窗格尚未開啟，請將它開啟 (F4)。
3. 在專案的 [屬性] 窗格中：
a) 將 [匿名驗證] 設定為 [停用]。
b) 將 [Windows 驗證] 設定為 [啟用]。

在 IIS 7 或更新版本中裝載：
1. 開啟 IIS 管理員，然後瀏覽至您的網站。
2. 在 [功能檢視] 中，按兩下 [驗證]。
3. 選取 [驗證] 頁面上的 [Windows 驗證]。若 Windows 
驗證並不在選項之中，您需要確定 Windows  驗證
已安裝在伺服器上。

若要在 Windows 上啟用 Windows  驗證：
a) 在 [控制台] 中開啟 [程式和功能]。
b) 選取 [開啟或關閉 Windows 功能]。
c) 瀏覽至 [搜尋程式及檔案] > [World Wide Web 服務] > [安全性]
並確定已勾選 Windows 驗證節點。

若要在 Windows 上啟用 Windows  驗證：
a) 在 Server Manager 中，選取 Web Server (IIS) 並按一下 [新增角色服務]。
b) 瀏覽至 [Web Server] > [安全性]
並確定已勾選 Windows 驗證節點。

4. 按一下 [動作] 窗格中的 [啟用]，即可使用 Windows 驗證。
5. 在 [驗證] 頁面上，選取 [匿名驗證]。
6. 在 [動作] 窗格中，按一下 [停用]，便能停用匿名驗證。