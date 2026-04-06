#!/bin/bash
export DEBIAN_FRONTEND=noninteractive
apt update
apt install -y --no-install-recommends     linux-image-amd64 live-boot systemd-sysv     xserver-xorg-core xserver-xorg-video-all libqt5widgets5     lightdm sudo curl ca-certificates

# Branding
echo "maxregnerOS" > /etc/hostname
sed -i 's/Debian/maxregnerOS 2029/g' /etc/os-release

# Install UI
cp /tmp/maxregnerUI /usr/local/bin/
chmod +x /usr/local/bin/maxregnerUI

# Session
mkdir -p /usr/share/xsessions
echo "[Desktop Entry]
Name=maxregnerUI
Exec=/usr/local/bin/maxregnerUI
Type=Application" > /usr/share/xsessions/maxregnerUI.desktop

echo "[Seat:*]
user-session=maxregnerUI" > /etc/lightdm/lightdm.conf
