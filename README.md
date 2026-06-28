<div align="center">

![logo](./assets/icon.svg)

# WandEnhancer

[![Original project](https://img.shields.io/badge/Original-k1tbyte%2FWand--Enhancer-blue?logo=github)](https://github.com/k1tbyte/Wand-Enhancer)
[![SignPath Code Signing](https://img.shields.io/badge/Code%20Signing-SignPath-blue?logo=signpath)](https://signpath.org/)

</div>

<h4>An open-source interoperability tool designed to extend local client-side configurations and improve the UX of the Wand application.</h4>

> This project is a fork of [k1tbyte/Wand-Enhancer](https://github.com/k1tbyte/Wand-Enhancer), the original project created by [k1tbyte](https://github.com/k1tbyte). It is maintained here at [Hachig0r0/Wand-Enhancer-Sign](https://github.com/Hachig0r0/Wand-Enhancer-Sign) under the Apache-2.0 license.

**🚨 IMPORTANT NOTICE: BEWARE OF SCAMMERS AND FAKE TUTORIALS 🚨**
The **ONLY** official place to download the prebuilt `WandEnhancer.exe` is from the [Releases](../../releases) tab of this exact GitHub repository. 
There are NO official YouTube tutorials or guides. Scammers frequently create fake tutorial videos using this project's name and place malware or password stealers in the description links. **If you downloaded this tool from a YouTube link, a Discord attachment, or any third-party website, delete it immediately. We are not responsible for third-party downloads.**

## 👾 Is it safe to use?

Yes. This project is entirely open-source, allowing anyone to audit the code. The patching runs locally on your machine and simply adjusts local client settings to enhance your user experience. The only network request is the update check against this repository's GitHub releases, which the built-in updater uses to offer new versions.

## 💫 What features are improved?

✅ Local environment configuration management <br/>
✅ Automated compatibility adjustments for new client versions <br/>
✅ Advanced layout and theme customization (Client-side only) <br/>
✅ AI Features <br/>
✅ Remote web panel (Remote Connect on mobile) <br/>

## 🌐 Remote Web Panel
WandEnhancer includes a built-in **Remote Web Panel** allowing you to control app features directly from your phone.

### Quick Start:
1. Ensure both your PC and phone are on the **same Wi-Fi network**.
2. Hover over the **Connect** button in the top bar of WandEnhancer.
3. Scan the displayed **QR code** with your phone's camera.

### Troubleshooting & Remote Access:
- **Page isn't loading?** First, ensure both your PC and phone are connected to the **same local network**. Some routers and guest Wi-Fi networks enable client isolation/AP isolation, which blocks devices on the same SSID from reaching each other. If it still does not load, check Windows Firewall and allow inbound traffic on TCP port `3223` for your local network. If Windows marked your connection as **Public**, switching it to **Private** can also help.
- **Using mobile data or a different network?** If you want to use the panel over mobile data (LTE/5G) or from an entirely different network, you can use [Tailscale](https://tailscale.com/) or similar VPN tools.

## 👀 How to use?

Getting started is simple. You no longer need to fork the repository to build your own binary.

1. Go to the **[Releases](../../releases)** page.
2. Download the latest `WandEnhancer.exe` from the assets section.
3. Extract the file if necessary, and run `WandEnhancer.exe` to apply your local client modifications.

> ⚠️ **Note on Windows SmartScreen:** Because the executable is newly compiled and unsigned, Windows Defender might show a warning. We are currently in the process of setting up **SignPath** to sign our binaries automatically. For now, you can safely click **"More info"** and then **"Run anyway"**.

## 🧩 Custom scripts

You can inject your own JavaScript into Wand at patch time to tweak or fix things in the client UI. This reuses the same renderer injection the Remote Web Panel uses, so it requires the **Remote Web Panel** patch to be enabled.

**How to add a script**

- In the patch dialog, add one or more `.js` files (only existing `.js` files are accepted), **or**
- Drop `.js` files into a `renderer-scripts/` folder placed next to the patcher executable.

Then patch as usual — your scripts are bundled into the client and run inside Wand's window.

**How it runs**

- Each script runs inside Wand's renderer (full DOM access, plus Node `require`).
- It is wrapped so a thrown error is logged and never crashes Wand.
- It may run **more than once** per launch (on load and again shortly after), so guard one‑time work behind a global flag.
- A small `WandEnhancer` helper is available: `WandEnhancer.log(...)`, `WandEnhancer.remoteUrl`, `WandEnhancer.apiVersion`.

**Minimal example** (`hello.js`)

```js
// Injected scripts can run multiple times — guard one-time setup.
if (!globalThis.__helloScriptInstalled) {
  globalThis.__helloScriptInstalled = true;

  WandEnhancer.log("Hello from my custom script!", WandEnhancer.remoteUrl);

  new MutationObserver(() => {
    const dialog = document.querySelector("ux-dialog:not([data-seen])");
    if (dialog) {
      dialog.setAttribute("data-seen", "1");
      WandEnhancer.log("A dialog opened.");
    }
  }).observe(document.documentElement, { childList: true, subtree: true });
}
```

> Scripts run with the same privileges as the Wand client. Only add scripts you trust and understand.

## 🛠️ How to build from source

Building from source on Windows requires a local development environment.

### Requirements

- `CMake`
- `Node.js` and `pnpm`
- `Visual Studio 2022` or `Build Tools for Visual Studio 2022` with `MSBuild`
- Visual Studio `Desktop development with C++` workload
- .NET Framework 4.8 desktop build tools / targeting pack

### Build steps

1. Clone this repository.
2. Install the requirements above and make sure `cmake`, `pnpm`, and `MSBuild` are available.
3. Run `build.cmd` from Command Prompt or PowerShell.

The build script installs the web panel dependencies, builds the frontend, compiles the native helper with CMake, restores NuGet packages, and builds the WPF solution.

---

## ❓ Q&A

- **Where do I download the executable?**
  - From the **[Releases](../../releases)** tab of this repository. The built-in updater can also fetch and install new versions for you. Do not download `.exe` files from YouTube descriptions, random mirrors, Discord attachments, or issue comments.
- **Why does Windows Defender or SmartScreen warn about it?**
  - The executable can be flagged because it modifies a local Electron app, which some scanners treat as suspicious, and unsigned builds always trigger SmartScreen. You can review the full source, build it yourself, or click **"More info"** then **"Run anyway"**. Code signing through **SignPath** is planned to remove these warnings.
- **Can I use a binary built by someone else?**
  - You can, but you should treat it as untrusted. Only the official releases from this repository are supported.
- **Does this send data anywhere?**
  - The patching work is local to your machine. The only network request is the update check against this repository's GitHub releases. The Remote Web Panel is served from your PC on your local network.

---
## 🖼️ Screenshots
![1](./assets/screenshots/app1.png)
<div align='center'>

![2](./assets/screenshots/app2.png)
</div>

---

## 📜 License
This project is licensed under the Apache-2.0 - see the [LICENSE](LICENSE.md) file for details.

---

Free code signing provided by [SignPath](https://signpath.org/).

> **Legal Disclaimer:**
> This project is a third-party enhancement tool intended solely for educational, research, and local interoperability purposes. It does not distribute any proprietary code or bypass server-side validations. All modifications are performed locally to customize the user's interface.

---

[![Star History Chart](https://api.star-history.com/svg?repos=Hachig0r0/Wand-Enhancer-Sign&type=Date)](https://www.star-history.com/#Hachig0r0/Wand-Enhancer-Sign&Date)
