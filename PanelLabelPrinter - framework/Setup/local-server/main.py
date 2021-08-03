#!/usr/bin/env python3
import sys
import os
import _thread
import subprocess

# from PyQt5.QtCore import QCoreApplication
# from PyQt5.QtGui import QIcon
# from PyQt5.QtWidgets import QWidget, QMessageBox, QSystemTrayIcon, QAction, QMenu, QApplication

import mdns
import api


# class MainWindow(QWidget):
#     def __init__(self, parent=None):
#         super(MainWindow, self).__init__(parent)

#     def alert(self, msg):
#         QMessageBox.warning(self, 'Error', msg, QMessageBox.Ok)


# def quit_app():
#     re = QMessageBox.question(m, "Warning", "Are you sure to exit?", QMessageBox.Yes |
#                               QMessageBox.No, QMessageBox.No)
#     if re == QMessageBox.Yes:
#         os.system('taskkill /F /IM barcodePrinter.exe')
#         QCoreApplication.instance().quit()
#         tp.setVisible(False)


if __name__ == '__main__':
    # app = QApplication(sys.argv)
    # o = OpenFileWindow()
    # if o.filename == '':
    #     sys.exit()
    # cmd = '../BarcodePrinter/BarcodePrinter/bin/x86/Debug/BarcodePrinter.exe'
    # subprocess.Popen(cmd)
    # print(cmd)
    # path = PurePosixPath(o.filename)
    # file_name = o.filename.split('/')[-1]
    mdns.broadcast()
    # print(path.parent, file_name)

    # QApplication.setQuitOnLastWindowClosed(False)
    # m = MainWindow()
    # tp = QSystemTrayIcon(m)
    # tp.setIcon(QIcon('icon.png'))

    # event_handler = watcher.monitor(str(path.parent), file_name)
    # event_handler.fileChanged.connect(m.alert)

    # a2 = QAction('&Exit', triggered=quit_app)
    # tpMenu = QMenu()
    # tpMenu.addAction(a2)
    # tp.setContextMenu(tpMenu)
    # tp.show()
    # tp.showMessage('Rently', 'Barcode verification service starts.', icon=0)

    # _thread.start_new_thread(server.app.run, ('0.0.0.0', 6000))
    api.app.run('0.0.0.0', 25348)
    sys.exit()
