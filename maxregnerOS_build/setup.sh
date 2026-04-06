#!/bin/bash
export DEBIAN_FRONTEND=noninteractive
apt update
apt install -y --no-install-recommends \
    linux-image-amd64 live-boot systemd-sysv \
    xserver-xorg-core xserver-xorg-video-all xserver-xorg-input-all \
    libqt5widgets5 lightdm openbox dbus-x11 sudo curl ca-certificates

# Branding
echo "maxregnerOS" > /etc/hostname
sed -i 's/Debian/maxregnerOS 2029/g' /etc/os-release

# Install UI
cp /tmp/maxregnerUI /usr/local/bin/
chmod +x /usr/local/bin/maxregnerUI

# Session Script
cat <<SES > /usr/local/bin/maxregner-session
#!/bin/bash
openbox &
/usr/local/bin/maxregnerUI
SES
chmod +x /usr/local/bin/maxregner-session

# Session Desktop Entry
mkdir -p /usr/share/xsessions
echo "[Desktop Entry]
Name=maxregnerUI
Exec=/usr/local/bin/maxregner-session
Type=Application" > /usr/share/xsessions/maxregnerUI.desktop

# Configure LightDM
mkdir -p /etc/lightdm
echo "[Seat:*]
user-session=maxregnerUI
autologin-user=maxregner" > /etc/lightdm/lightdm.conf

# Add user
useradd -m -s /bin/bash maxregner
echo "maxregner:maxregner" | chpasswd
usermod -aG sudo maxregner
