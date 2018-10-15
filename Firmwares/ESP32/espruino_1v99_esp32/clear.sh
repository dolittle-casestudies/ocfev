#!/bin/bash
esptool.py    \
        --chip esp32                                \
        --port /dev/tty.SLAB_USBtoUART                   \
        --baud 921600                               \
        erase_flash
