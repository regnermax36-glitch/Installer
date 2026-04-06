#!/bin/bash
# maxregnerOS 2029 - Master Build Script
set -e

WORK_DIR=$(pwd)/maxregner_work
CHROOT_DIR=$WORK_DIR/chroot
ISO_DIR=$WORK_DIR/iso

mkdir -p $CHROOT_DIR $ISO_DIR/live $ISO_DIR/boot/grub

# 1. Compile maxregnerUI
echo "Compiling maxregnerUI..."
cd src/maxregnerUI && qmake && make
cd ../..

# 2. Bootstrap Debian Sid
echo "Bootstrapping Debian Sid..."
sudo debootstrap --variant=minbase --arch amd64 sid $CHROOT_DIR http://deb.debian.org/debian/

# 3. Apply Customizations
echo "Applying customizations..."
cp src/maxregnerUI/maxregnerUI $CHROOT_DIR/tmp/
cp setup.sh $CHROOT_DIR/tmp/
sudo chroot $CHROOT_DIR bash /tmp/setup.sh

# 4. Build Squashfs
echo "Creating Squashfs..."
sudo mksquashfs $CHROOT_DIR $ISO_DIR/live/filesystem.squashfs -comp xz

# 5. Prepare Kernel and Initrd
cp $CHROOT_DIR/boot/vmlinuz-* $ISO_DIR/live/vmlinuz
cp $CHROOT_DIR/boot/initrd.img-* $ISO_DIR/live/initrd

# 6. GRUB Configuration
cat <<GRUB > $ISO_DIR/boot/grub/grub.cfg
set default=0
set timeout=1
menuentry "maxregnerOS 2029" {
    linux /live/vmlinuz boot=live quiet
    initrd /live/initrd
}
GRUB

# 7. Generate Bootable ISO (Hybrid BIOS/UEFI)
echo "Generating ISO..."
grub-mkrescue -o maxregnerOS_2029.iso $ISO_DIR

echo "Build complete: maxregnerOS_2029.iso"
