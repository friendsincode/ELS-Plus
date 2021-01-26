--[[
    ELS Plus - A ELS implementation for FiveM
    Copyright (C) 2017  E.J. Bevenour

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
]]
fx_version "bodacious"
games {"gta5"}

local NUIEnabled = true

name "ELS Plus"
description "Fully synced version of ELS Plus for FiveM"
author "Friends in Code: Dex's Lab"
version "0.3.2.172"
url "https://git.friendsincode.com/friendsincode-public/elsplus"

-- ELS Plus Client Scripts
client_script "els-plus.net.dll"

-- Debug symbols
files(
    {
        "els-plus.net.dll.mdb",
        "els-server.net.dll.mdb"
    }
)
-- NUI Stuff
if NUIEnabled then
    ui_page("NUI/app/index.html")

    files(
        {
            "NUI/app/index.html",
            "NUI/app/js/ui.js",
            "NUI/app/js/sirenclick.ogg",
            "NUI/app/js/sirenclickoff.ogg",
            "NUI/app/js/indon.ogg",
            "NUI/app/js/indoff.ogg",
            "NUI/app/js/jquery-0.3.2.172min.js",
            -- New Hotness
            "NUI/app/newhotness/index.html",
            "NUI/app/newhotness/js/popper.js",
            "NUI/app/newhotness/js/bootstrap.bundle.min.js",
            "NUI/app/newhotness/js/bootstrap-toggle.min.js",
            "NUI/app/newhotness/js/main.js",
            "NUI/app/newhotness/css/bootstrap.min.css",
            "NUI/app/newhotness/css/bootstrap-toggle.min.css",
            "NUI/app/newhotness/css/main.css",
            -- Old and Busted (JK love the ui Arthur)
            "NUI/app/oldandbusted/css/main.css",
            "NUI/app/oldandbusted/js/main.js",
            "NUI/app/oldandbusted/index.html",
            --Whelen UI
            "NUI/app/whelen/index.html",
            "NUI/app/whelen/css/main.css",
            "NUI/app/whelen/css/bootstrap.min.css",
            "NUI/app/whelen/css/bootstrap-slider.min.css",
            "NUI/app/whelen/images/2tone.png",
            "NUI/app/whelen/images/aux1.png",
            "NUI/app/whelen/images/aux2.png",
            "NUI/app/whelen/images/blank.png",
            "NUI/app/whelen/images/cruise.png",
            "NUI/app/whelen/images/horn.png",
            "NUI/app/whelen/images/man1.png",
            "NUI/app/whelen/images/man2.png",
            "NUI/app/whelen/images/scene.png",
            "NUI/app/whelen/images/stby.png",
            "NUI/app/whelen/images/tkdn.png",
            "NUI/app/whelen/images/wail.png",
            "NUI/app/whelen/images/whelen_logo.png",
            "NUI/app/whelen/images/wigwag.png",
            "NUI/app/whelen/images/yelp.png",
            "NUI/app/whelen/js/popper.js",
            "NUI/app/whelen/js/bootstrap.bundle.min.js",
            "NUI/app/whelen/js/bootstrap-slider.min.js",
            "NUI/app/whelen/js/main.js"
        }
    )
end

-- ELS Plus Server Scripts
server_script "els-server.net.dll"

files(
    {
        "ELS.ini"
        "SharpConfig.net.dll",
        "Newtonsoft.Json.dll"
    }
)

dependencies {"baseevents"}
