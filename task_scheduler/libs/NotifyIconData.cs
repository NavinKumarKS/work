﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace task_scheduler.libs
{
    public class WindowMessageSink
    {

        /// <summary>
        /// The ID of messages that are received from the the
        /// taskbar icon.
        /// </summary>
        public const int CallbackMessageId = 0x400;

    }

    public enum NotifyIconVersion
    {
        /// <summary>
        /// Default behavior (legacy Win95). Expects
        /// a <see cref="NotifyIconData"/> size of 488.
        /// </summary>
        Win95 = 0x0,

        /// <summary>
        /// Behavior representing Win2000 an higher. Expects
        /// a <see cref="NotifyIconData"/> size of 504.
        /// </summary>
        Win2000 = 0x3,

        /// <summary>
        /// Extended tooltip support, which is available for Vista and later.
        /// Detailed information about what the different versions do, can be found <a href="https://docs.microsoft.com/en-us/windows/win32/api/shellapi/nf-shellapi-shell_notifyicona">here</a>
        /// </summary>
        Vista = 0x4
    }

    /// <summary>
    /// Indicates which members of a <see cref="NotifyIconData"/> structure
    /// were set, and thus contain valid data or provide additional information
    /// to the ToolTip as to how it should display.
    /// </summary>
    [Flags]
    public enum IconDataMembers
    {
        /// <summary>
        /// The message ID is set.
        /// </summary>
        Message = 0x01,

        /// <summary>
        /// The notification icon is set.
        /// </summary>
        Icon = 0x02,

        /// <summary>
        /// The tooltip is set.
        /// </summary>
        Tip = 0x04,

        /// <summary>
        /// State information (<see cref="IconState"/>) is set. This
        /// applies to both <see cref="NotifyIconData.IconState"/> and
        /// <see cref="NotifyIconData.StateMask"/>.
        /// </summary>
        State = 0x08,

        /// <summary>
        /// The balloon ToolTip is set. Accordingly, the following
        /// members are set: <see cref="NotifyIconData.BalloonText"/>,
        /// <see cref="NotifyIconData.BalloonTitle"/>, <see cref="NotifyIconData.BalloonFlags"/>,
        /// and <see cref="NotifyIconData.VersionOrTimeout"/>.
        /// </summary>
        Info = 0x10,

        // Internal identifier is set. Reserved, thus commented out.
        //Guid = 0x20,

        /// <summary>
        /// Windows Vista (Shell32.dll version 6.0.6) and later. If the ToolTip
        /// cannot be displayed immediately, discard it.<br/>
        /// Use this flag for ToolTips that represent real-time information which
        /// would be meaningless or misleading if displayed at a later time.
        /// For example, a message that states "Your telephone is ringing."<br/>
        /// This modifies and must be combined with the <see cref="Info"/> flag.
        /// </summary>
        Realtime = 0x40,

        /// <summary>
        /// Windows Vista (Shell32.dll version 6.0.6) and later.
        /// Use the standard ToolTip. Normally, when uVersion is set
        /// to NOTIFYICON_VERSION_4, the standard ToolTip is replaced
        /// by the application-drawn pop-up user interface (UI).
        /// If the application wants to show the standard tooltip
        /// in that case, regardless of whether the on-hover UI is showing,
        /// it can specify NIF_SHOWTIP to indicate the standard tooltip
        /// should still be shown.<br/>
        /// Note that the NIF_SHOWTIP flag is effective until the next call 
        /// to Shell_NotifyIcon.
        /// </summary>
        UseLegacyToolTips = 0x80
    }
    /// <summary>
    /// Flags that define the icon that is shown on a balloon
    /// tooltip.
    /// </summary>
    public enum BalloonFlags
    {
        /// <summary>
        /// No icon is displayed.
        /// </summary>
        None = 0x00,

        /// <summary>
        /// An information icon is displayed.
        /// </summary>
        Info = 0x01,

        /// <summary>
        /// A warning icon is displayed.
        /// </summary>
        Warning = 0x02,

        /// <summary>
        /// An error icon is displayed.
        /// </summary>
        Error = 0x03,

        /// <summary>
        /// Windows XP Service Pack 2 (SP2) and later.
        /// Use a custom icon as the title icon.
        /// </summary>
        User = 0x04,

        /// <summary>
        /// Windows XP (Shell32.dll version 6.0) and later.
        /// Do not play the associated sound. Applies only to balloon ToolTips.
        /// </summary>
        NoSound = 0x10,

        /// <summary>
        /// Windows Vista (Shell32.dll version 6.0.6) and later. The large version
        /// of the icon should be used as the balloon icon. This corresponds to the
        /// icon with dimensions SM_CXICON x SM_CYICON. If this flag is not set,
        /// the icon with dimensions XM_CXSMICON x SM_CYSMICON is used.<br/>
        /// - This flag can be used with all stock icons.<br/>
        /// - Applications that use older customized icons (NIIF_USER with hIcon) must
        ///   provide a new SM_CXICON x SM_CYICON version in the tray icon (hIcon). These
        ///   icons are scaled down when they are displayed in the System Tray or
        ///   System Control Area (SCA).<br/>
        /// - New customized icons (NIIF_USER with hBalloonIcon) must supply an
        ///   SM_CXICON x SM_CYICON version in the supplied icon (hBalloonIcon).
        /// </summary>
        LargeIcon = 0x20,

        /// <summary>
        /// Windows 7 and later.
        /// </summary>
        RespectQuietTime = 0x80
    }

    public enum IconState
    {
        /// <summary>
        /// The icon is visible.
        /// </summary>
        Visible = 0x00,

        /// <summary>
        /// Hide the icon.
        /// </summary>
        Hidden = 0x01,

        // The icon is shared - currently not supported, thus commented out.
        //Shared = 0x02
    }
    /// <summary>
    /// A struct that is submitted in order to configure
    /// the taskbar icon. Provides various members that
    /// can be configured partially, according to the
    /// values of the <see cref="IconDataMembers"/>
    /// that were defined.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct NotifyIconData
    {
        /// <summary>
        /// Size of this structure, in bytes.
        /// </summary>
        public uint cbSize;

        /// <summary>
        /// Handle to the window that receives notification messages associated with an icon in the
        /// taskbar status area. The Shell uses hWnd and uID to identify which icon to operate on
        /// when Shell_NotifyIcon is invoked.
        /// </summary>
        public IntPtr WindowHandle;

        /// <summary>
        /// Application-defined identifier of the taskbar icon. The Shell uses hWnd and uID to identify
        /// which icon to operate on when Shell_NotifyIcon is invoked. You can have multiple icons
        /// associated with a single hWnd by assigning each a different uID. This feature, however
        /// is currently not used.
        /// </summary>
        public uint TaskbarIconId;

        /// <summary>
        /// Flags that indicate which of the other members contain valid data. This member can be
        /// a combination of the NIF_XXX constants.
        /// </summary>
        public IconDataMembers ValidMembers;

        /// <summary>
        /// Application-defined message identifier. The system uses this identifier to send
        /// notifications to the window identified in hWnd.
        /// </summary>
        public uint CallbackMessageId;

        /// <summary>
        /// A handle to the icon that should be displayed. Just
        /// <c>Icon.Handle</c>.
        /// </summary>
        public IntPtr IconHandle;

        /// <summary>
        /// String with the text for a standard ToolTip. It can have a maximum of 64 characters including
        /// the terminating NULL. For Version 5.0 and later, szTip can have a maximum of
        /// 128 characters, including the terminating NULL.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string ToolTipText;


        /// <summary>
        /// State of the icon. Remember to also set the <see cref="StateMask"/>.
        /// </summary>
        public IconState IconState;

        /// <summary>
        /// A value that specifies which bits of the state member are retrieved or modified.
        /// For example, setting this member to <see cref="TaskbarNotification.Interop.IconState.Hidden"/>
        /// causes only the item's hidden
        /// state to be retrieved.
        /// </summary>
        public IconState StateMask;

        /// <summary>
        /// String with the text for a balloon ToolTip. It can have a maximum of 255 characters.
        /// To remove the ToolTip, set the NIF_INFO flag in uFlags and set szInfo to an empty string.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string BalloonText;

        /// <summary>
        /// Mainly used to set the version when <see cref="WinApi.Shell_NotifyIcon"/> is invoked
        /// with <see cref="NotifyCommand.SetVersion"/>. However, for legacy operations,
        /// the same member is also used to set timeouts for balloon ToolTips.
        /// </summary>
        public uint VersionOrTimeout;

        /// <summary>
        /// String containing a title for a balloon ToolTip. This title appears in boldface
        /// above the text. It can have a maximum of 63 characters.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string BalloonTitle;

        /// <summary>
        /// Adds an icon to a balloon ToolTip, which is placed to the left of the title. If the
        /// <see cref="BalloonTitle"/> member is zero-length, the icon is not shown.
        /// </summary>
        public BalloonFlags BalloonFlags;

        /// <summary>
        /// Windows XP (Shell32.dll version 6.0) and later.<br/>
        /// - Windows 7 and later: A registered GUID that identifies the icon.
        ///   This value overrides uID and is the recommended method of identifying the icon.<br/>
        /// - Windows XP through Windows Vista: Reserved.
        /// </summary>
        public Guid TaskbarIconGuid;

        /// <summary>
        /// Windows Vista (Shell32.dll version 6.0.6) and later. The handle of a customized
        /// balloon icon provided by the application that should be used independently
        /// of the tray icon. If this member is non-NULL and the <see cref="TaskbarNotification.Interop.BalloonFlags.User"/>
        /// flag is set, this icon is used as the balloon icon.<br/>
        /// If this member is NULL, the legacy behavior is carried out.
        /// </summary>
        public IntPtr CustomBalloonIconHandle;


        /// <summary>
        /// Creates a default data structure that provides
        /// a hidden taskbar icon without the icon being set.
        /// </summary>
        /// <param name="handle"></param>
        /// <returns>NotifyIconData</returns>
        public static NotifyIconData CreateDefault(IntPtr handle)
        {
            var data = new NotifyIconData();

            if (Environment.OSVersion.Version.Major >= 6)
            {
                //use the current size
                data.cbSize = (uint)Marshal.SizeOf(data);
            }
            else
            {
                //we need to set another size on xp/2003- otherwise certain
                //features (e.g. balloon tooltips) don't work.
                data.cbSize = 952; // NOTIFYICONDATAW_V3_SIZE

                //set to fixed timeout
                data.VersionOrTimeout = 10;
            }

            data.WindowHandle = handle;
            data.TaskbarIconId = 0x0;
            data.CallbackMessageId = WindowMessageSink.CallbackMessageId;
            data.VersionOrTimeout = (uint)NotifyIconVersion.Win95;

            data.IconHandle = IntPtr.Zero;

            //hide initially
            data.IconState = IconState.Hidden;
            data.StateMask = IconState.Hidden;

            //set flags
            data.ValidMembers = IconDataMembers.Message
                                | IconDataMembers.Icon
                                | IconDataMembers.Tip;

            //reset strings
            data.ToolTipText = data.BalloonText = data.BalloonTitle = string.Empty;

            return data;
        }
    }
}
