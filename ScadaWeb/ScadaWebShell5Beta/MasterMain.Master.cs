﻿/*
 * Copyright 2016 Mikhail Shiryaev
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 * 
 * Product  : Rapid SCADA
 * Module   : SCADA-Web
 * Summary  : Main master page
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2016
 * Modified : 2016
 */

using Scada.Web.Shell;
using System;
using System.Text;

namespace Scada.Web
{
    /// <summary>
    /// Main master page
    /// <para>Основная страница-шаблон</para>
    /// </summary>
    public partial class MasterMain : System.Web.UI.MasterPage
    {
        // Дерево представлений
        private const string FolderImageUrl = "~/images/treeview/folder.png";
        private const string DocumentImageUrl = "~/images/treeview/document.png";
        private static readonly TreeViewRenderer treeViewRenderer =
            new TreeViewRenderer();

        private AppData appData;    // общие данные веб-приложения
        private UserData userData;  // данные пользователя приложения

        protected bool mainMenuVisible; // отобразить главное меню при загрузке страницы


        /// <summary>
        /// Установить видимость главного меню
        /// </summary>
        private void SetMainMenuVisible()
        {
            mainMenuVisible = !string.Equals(Request.Url.AbsolutePath, ResolveUrl("~/View.aspx"), 
                StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Генерировать HTML-код главного меню
        /// </summary>
        protected string GenerateMainMenuHtml()
        {
            TreeViewRenderer.Options options = new TreeViewRenderer.Options() { ShowIcons = false };
            return treeViewRenderer.GenerateHtml(userData.UserMenu.MenuItems, Request.Url.AbsolutePath, options);
        }

        /// <summary>
        /// Генерировать HTML-код проводника представлений
        /// </summary>
        protected string GenerateExplorerHtml()
        {
            TreeViewRenderer.Options options = new TreeViewRenderer.Options()
            {
                ShowIcons = true,
                FolderImageUrl = ResolveUrl(FolderImageUrl),
                DocumentImageUrl = ResolveUrl(DocumentImageUrl)
            };
            return treeViewRenderer.GenerateHtml(userData.UserViews.ViewNodes, null, options);
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            appData = AppData.GetAppData();
            userData = UserData.GetUserData();
            userData.CheckLoggedOn(true);

            SetMainMenuVisible();
        }

        protected void lbtnMainLogout_Click(object sender, EventArgs e)
        {
            // выход из системы
            userData.Logout();
            appData.RememberMe.ForgetUser(Context);
            Response.Redirect("~/Login.aspx");
        }
    }
}