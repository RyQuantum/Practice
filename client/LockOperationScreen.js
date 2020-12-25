import React from 'react';
import {
  StyleSheet,
  View,
  Text,
  TouchableHighlight,
  ScrollView,
  Alert,
  Switch,
  Slider,
  PermissionsAndroid,
  Platform,
  Modal,
  Dimensions,
} from 'react-native';
import Dialog from "react-native-dialog";
import { CheckBox } from 'react-native-elements';
import { getLockDetails, getFirmwareUrl, createCyclicPasscode, createPeriodCode, createFOB } from '../services/APIManager';
import { lockType, buttonColor, button2Color } from '../services/Constants';
import LoadingIndicator from '../components/LoadingIndicator';
import Overlay from 'react-native-modal-overlay';
import { objRNOaksOpenMobileLib } from "./HomeScreen";
import axios from 'axios';
import RNFetchBlob from 'rn-fetch-blob';
import { NordicDFU, DFUEmitter } from 'react-native-nordic-dfu';
import RNFS from 'react-native-fs';
import Spinner from 'react-native-loading-spinner-overlay';
import ProgressBar from 'react-native-progress/Bar';
import { Row, Table } from 'react-native-table-component';
import { alertIOS, convertCycleTypeToBinary } from '../services/Utilities';

const initializeError = 'Please initialize the lock to perform lock operations'
export const getTodaysDate = () => {
  let d = new Date();
  let year = d.getYear() + 1900;
  let month = d.getMonth() + 1;
  if (month <= 9) {
    month = '0' + month;
  }
  let date = d.getDate();
  if (date <= 9) {
    date = '0' + date;
  }
  return `${year}-${month}-${date}`;
}

export const getNextWeekDate = () => {
  let d = new Date();
  d.setDate(d.getDate() + 7);
  let year = d.getYear() + 1900;
  let month = d.getMonth() + 1;
  if (month <= 9) {
    month = '0' + month;
  }
  let date = d.getDate();
  if (date <= 9) {
    date = '0' + date;
  }
  return `${year}-${month}-${date}`;
}

export const getTodaysTime = () => {
  let d = new Date();
  return d.getTime()
}

export const getNextWeekTime = () => {
  let d = new Date();
  d.setDate(d.getDate() + 7);
  return d.getTime()
}

class LockOperationScreen extends React.Component {
  static navigationOptions = ({ navigation }) => ({
    title: navigation.state.params.title,
    headerBackTitle: null,
    headerBackTitleStyle: { color: buttonColor },
  });

  constructor(props) {
    super(props);
    this.state = {
      showActivity: false,
      dst: false,
      masterCode: '',
      dialogVisible: false,
      autoLockDialogVisisble: false,
      installationPopup: false,
      installationDialogVisible: false,
      cyclicPopup: false,
      cyclicDaysPopup: false,
      cycleDays: [false,false,false,false,false,false,false],
      customCode: '112233',
      codeName: 'Test Cyclic Pwd',
      codeStartDate: getTodaysDate(),
      codeEndDate: getNextWeekDate(),
      codeStartTime: '00:00:00',
      codeEndTime: '23:59:59',
      doorSensorDialogVisible: false,
      isAutoLockEnabled: false,
      isSensorEnabled: false,
      isDoorSensorSupport: false,
      icCardSupported: undefined,
      isAddFOB:false,
      autoLockInfo: {},
      isAddCPUFOBWithNo: false,
      isAddFOBWithNo: false,
      isDeleteFOB : false,
      isPassageModeEnabled: false,
      isSetPassageMode: false,
      isResetButtonEnabled: false,
      setResetButtonDialogVisible: false,
      autoLockTimes:[
        {title:'5 seconds', value:5},{title:'10 seconds', value:10},
        {title:'15 seconds', value:15},{title:'30 seconds', value:30},{title:'1 min', value:60},
      ],
      lock: this.props.navigation.state.params.item,
      blePlugin: this.props.navigation.state.params.blePlugin,
      settingMode: this.props.navigation.state.params.item.settingMode,
      DFUDialogVisible: false,
      DFUInProgress: false,
      firmwareVersion: '',
      DFUProgressBar: false,
      DFUProgressPercent: '0',
      DFUStates1: [],
      DFUStates2: [],
      reportHead: ['', 'results', 'message'],
      reportData: [
        ['1', '️', ''],
        ['2', '️', ''],
        ['3', '️', ''],
        ['4', '', ''],
        ['5', '', ''],
        ['6', '', ''],
      ],
    };
    axios.defaults.headers['Authorization'] = `Bearer ${this.state.lock.deviceAuthToken}`;
    this.times = 0;
  }

  componentWillUnmount() {
    if (this.props.navigation.state.params.onGoBack) {
      this.props.navigation.state.params.onGoBack();
    }
  }

  componentDidMount() {
    this.props.navigation.setParams({
      title: this.state.lock.lockType + ' : ' + this.state.lock.name,
    });
    if (!this.state.settingMode) {
      getLockDetails(this.state.lock.lockMac, this.state.lock.deviceAuthToken)
        .then((lockKeyInfo) => {
          if (lockKeyInfo) {
            const { masterCode = '' } = lockKeyInfo;
            this.setState({ masterCode });
          }
          else {
            alert('Lock info not found')
          }
        })
        .catch((error) => {
          alert(error.message)
        });
      this.isSupportFOB()
    }
  }

  initLock() {
    this.setState({ showActivity: true });
    const {lock} = this.state;
    lock.addAdministrator().then((response) => {
      if (response.success) {
        this.setState({
          showActivity: false,
          masterCode: response.masterCode,
          settingMode:false,
        });
        alert('Lock initialised Successfully');
        this.isSupportFOB()
      }
      else {
        this.setState({ showActivity: false });
        alert(response.message)
      }
    }).catch(error => {
      this.setState({ showActivity: false });
      alert(error.message);
    });
  }

  readLockTime() {
    if (this.state.lock) {
      this.setState({ showActivity: true });
      this.state.lock.getLockTime().then((response) => {
        this.setState({ showActivity: false });
        if (response.success) {
          const str = parseInt(response.timestamp, 16).toString(2).padStart(32, '0');
          const year = '20' + parseInt(str.slice(0, 6), 2).toString().padStart(2, '0');
          const mon = parseInt(str.slice(6, 10), 2).toString().padStart(2, '0');
          const day = parseInt(str.slice(10, 15), 2).toString().padStart(2, '0');
          const hour = parseInt(str.slice(15, 20), 2).toString().padStart(2, '0');
          const min = parseInt(str.slice(20, 26), 2).toString().padStart(2, '0');
          const sec = parseInt(str.slice(26, 32), 2).toString().padStart(2, '0');

          alert(`Lock Time: ${year} ${mon}-${day}, ${hour}:${min}:${sec} DST: ${response.dst}`)
        }
        else {
          alert('Error:' + response.message)
        }
        // (response.success) ? alert('screens Time:' + response.time) : alert('Error:' + response.message)
      }).catch(error => {
        this.setState({ showActivity: false });
        alert(error.message);
      })
    }
    else {
      alert(initializeError)
    }
  }

  resetLock() {
    if (this.state.lock) {
      this.setState({ showActivity: true });
      this.state.lock.resetLock().then((response) => {
        this.setState({ showActivity: false })
        if (response.success) {
          this.setState({ lock: {} });
          setTimeout(() => {
            this.props.navigation.goBack();
            alert('Lock reset successfully.')
          },500);
        }
        else {
          alert(response.message)
        }
      }).catch(error => {
        this.setState({ showActivity: false })
        alert(error.message);
      })
    }
    else {
      alert(initializeError)
    }
  }

  returnLockTimeInfo(time) {
    let timeObject = {}
    timeObject.title = (Math.floor(time / 60) >= 1) ? Math.floor(time / 60) + ' min' : time + ' seconds'
    timeObject.value = time
    return timeObject
  }

  readAutoLockTime(showAlert) {
    if (this.state.lock) {
      this.setState({ showActivity: true });
      this.state.lock.getAutoLockTime().then((response) => {
        this.setState({ showActivity: false })
        if (response.success) {
          const timeInfo = {
            current: response.current,
            max: response.maximum,
            min: response.minimum
          }
          if (!showAlert) {
            var sliderArray = []
            let lastObjectArr = this.state.autoLockTimes[this.state.autoLockTimes.length - 1]
            if (timeInfo.max > lastObjectArr.value) {
              this.state.autoLockTimes.push(this.returnLockTimeInfo(timeInfo.max))
            }
            else {
              for (var index in this.state.autoLockTimes) {
                if (this.state.autoLockTimes[index].value <= timeInfo.max) {
                  sliderArray.push(this.state.autoLockTimes[index])
                }
              }
              this.setState({ autoLockTimes: sliderArray })
            }
            this.setState({
              autoLockInfo: this.returnLockTimeInfo(timeInfo.current),
              autoLockDialogVisisble: true,
              isAutoLockEnabled: (timeInfo.current > 0)
            })
          }
          if (timeInfo.current === 0) {
            if (showAlert)
              alert('Auto Lock is disabled')
          }
          else {
            if (showAlert) {
              let autoLockTimeString = timeInfo.current + ' seconds'
              alert('Auto Lock Time is set to ' + autoLockTimeString)
            }
          }
        }
        else {
          alert(initializeError)
        }
      }).catch(error => {
        this.setState({ showActivity: false })
        alert(error.message);
      });
    }
  }

  setAutoLockTime() {
    this.readAutoLockTime(false)
  }

  submitAutoLockTimeAction() {
    this.setState({ autoLockDialogVisisble: false, showActivity: true });
    let currentLockTime = (this.state.isAutoLockEnabled) ? this.state.autoLockInfo.value : 0;
    console.log('currentLockTime',currentLockTime);
    this.state.lock.setAutoLockTime(currentLockTime).then((response) => {
      this.setState({ showActivity: false })
      alert(response.message)
    }).catch(error => {
      this.setState({ showActivity: false })
      alert(error.message);
    })
  }

  isSupportFOB() {
        this.state.lock.isSupportIC().then(({ icCardSupported }) => {
        this.setState({ icCardSupported });
      })
      .catch((error) => {
        alert(error.message)
       })
  }

  setResetButton = () => this.setState({ setResetButtonDialogVisible: true });

  doorSensorAction() {
    if (this.state.lock) {
      this.setState({ showActivity: true });
      this.state.lock.isSupportDoorSensor().then(({ doorSensorSupported }) => {
        if (doorSensorSupported) {
          this.setState({ isDoorSensorSupport: doorSensorSupported})
          this.state.lock.isDoorSensorEnabled().then(({ doorSensorEnabled }) => {
            this.setState({ showActivity: false, isSensorEnabled: doorSensorEnabled, doorSensorDialogVisible: doorSensorSupported});
          }).catch((error) => {
            this.setState({ showActivity: false });
            alert(error.message)
           })
        }
        else {
          this.setState({ showActivity: false });
          alert('Door sensor not supported')
        }
       }).catch((error) => {
        this.setState({ showActivity: false });
        alert(error.message)
       })
    }
    else {
      alert(initializeError)
    }
  }

  submitDoorSensorEnabledAction() {
    if (this.state.lock) {
      this.setState({ doorSensorDialogVisible: false, showActivity: true })
      this.state.lock.setDoorSensorLocking(this.state.isSensorEnabled).then((response) => {
        this.setState({ showActivity: false })
        alert(response.message)
      }).catch((error) => {
        this.setState({ showActivity: false })
        alert(error.message)
      })
    }
    else {
      alert(initializeError)
    }
  }

  submitResetButtonEnabledAction = async () => {
    this.setState({ setResetButtonDialogVisible: false, showActivity: true });
    try {
      const res = await this.state.lock.setResetButton(this.state.isResetButtonEnabled);
      this.setState({ showActivity: false })
      alertIOS(res.message);
    } catch (err) {
      this.setState({ showActivity: false })
      alertIOS(err.message);
    }
  };

  getBatteryInfo() {
    if (this.state.lock) {
      this.setState({showActivity: true});
      const listener = ({ lockMac, battery }) => {
        if (lockMac !== this.state.lock.lockMac) return;
        alert('Battery Status: ' + battery + '%');
        objRNOaksOpenMobileLib.stopScan();
        objRNOaksOpenMobileLib.removeListener('foundDevice', listener);
        clearTimeout(timeoutID);
        this.setState({showActivity: false});
      };
      objRNOaksOpenMobileLib.on('foundDevice', listener);
      objRNOaksOpenMobileLib.startScan();
      const timeoutID = setTimeout(() => {
        alert('Device not found');
        objRNOaksOpenMobileLib.removeListener('foundDevice', listener);
        this.setState({showActivity: false});
      }, 10000);
    } else {
      alert(initializeError)
    }
  }


  createCyclicCode() {
    if (this.state.lock) {
      this.setState({cyclicPopup: true, isAddFOB : false, customCode:'' , isSetPassageMode: false });
    }
    else {
      alert(initializeError)
    }
  }

  addCard() {
    if (this.state.lock) {
      this.setState({cyclicPopup: true , isAddFOB : true, codeName : 'Test FOB', isAddCPUFOBWithNo : false, isAddFOBWithNo:false, isSetPassageMode: false });
    }
    else {
      alert(initializeError)
    }
  }

  addCardWithNo() {
    if (this.state.lock) {
      this.setState({
        cyclicPopup: true,
        isAddFOB: true,
        codeName: 'Test FOB With No',
        isAddCPUFOBWithNo: false,
        isAddFOBWithNo: true,
        customCode: '',
        isSetPassageMode: false,
      });
    } else {
      alert(initializeError);
    }
  }

  addCPUCardWithNo() {
    if (this.state.lock) {
      if(this.state.lock.lockType == lockType.V3LOCK) {
        this.setState({cyclicPopup: true , isAddFOB : true, codeName : 'Test FOB With No', isAddCPUFOBWithNo: true,isAddFOBWithNo: false, customCode: '', isSetPassageMode: false });
      }
      else {
        alert('Lock does not support add fob by fob number operation')
      }
    }
    else {
      alert(initializeError)
    }
  }

  doLockUnlockOperation = async op => {
    try {
      return await this.state.lock[op]();
    } catch (err) {
      return err;
    }
  }

  async testInstallationState() {
    try {
      this.setState({ showActivity: true });
      const reportData = [];
      for(let i = 1; i <= 6; i++) {
        const op = i % 2 === 0 ? 'unlock' : 'lock';
        const { success, message, code } = await this.doLockUnlockOperation(op);
        reportData.push([op, success ? '✔️' : '❌', success? message : (`lockStatus: ${(code).toString(16).toUpperCase()}, ${message}`)]);
      }
      this.setState({ showActivity: false, reportData, installationDialogVisible: true });
    } catch (error) {
      this.setState({ showActivity: false })
      alert(error.message);
    }
  }

  displayInstallationReport = () => {
    if (!this.state.installationDialogVisible) return null;
    return (
      <Dialog.Container visible="true">
        <Dialog.Title>Report</Dialog.Title>
        <Table borderStyle={{borderWidth: 2, borderColor: '#c8e1ff'}}>
          <Row data={this.state.reportHead} flexArr={[2, 2, 5]} style={{ height: 40, backgroundColor: '#f1f8ff' }} textStyle={{ margin: 6, textAlign: 'center' }}/>
          {this.state.reportData.map(rowDate => <Row data={rowDate} flexArr={[2, 2, 5]} textStyle={{ margin: 6, textAlign: 'center' }} style={{height: 40}}/>)}
        </Table>
        <Dialog.Button
          label="OK"
          onPress={() => this.setState({ installationDialogVisible: false })}
        />
      </Dialog.Container>
    );
  }

  displayPassageModeDialog() {
    if (this.state.lock) {
      if(this.state.lock.lockType == lockType.V3LOCK) {
        this.setState({ cyclicPopup: true, isAddFOB: false, isSetPassageMode: true });
      }
      else {
        alert('Lock does not support add fob by fob number operation')
      }
    }
    else {
      alert(initializeError)
    }
  }

  deletePasscodeAction(isDeleteFOB) {
    if (this.state.lock) {
      this.props.navigation.navigate('DeleteScreen', {
        isDeleteFOB: isDeleteFOB,
        lock: this.state.lock,
        deviceToken: this.state.lock.deviceAuthToken,
        onGoBack: this.onGoBack
      });
    }
    else {
      alert(initializeError)
    }
  }

  unlockDevice() {
    if (this.state.lock) {
      this.setState({ showActivity: true });
      this.state.lock.unlock().then((response) => {
        this.setState({ showActivity: false })
        alert(response.message);
      }).catch(error => {
        this.setState({ showActivity: false })
        alert(error.message);
      })
    }
    else {
      alert(initializeError)
    }
  }

  lockDevice() {
    if (this.state.lock) {
      this.setState({ showActivity: true });
      this.state.lock.lock().then((response) => {
        this.setState({ showActivity: false })
        alert(response.message)
      }).catch(error => {
        this.setState({ showActivity: false })
        alert(error.message);
      })
    }
    else {
      alert(initializeError)
    }
  }

  triggerMemfault = async () => {
    const { lock: { lockMac }, blePlugin } = this.state;
    const deviceId = blePlugin.getDeviceId(lockMac);
    try {
      this.setState({ showActivity: true });
      await blePlugin.connectToDevice(deviceId);
      let count = 0; const arr = [];
      await new Promise((resolve, reject) => {
        setTimeout(() => reject(monitor.remove()), 30000);
        const monitor = blePlugin.manager.monitorCharacteristicForDevice(
          deviceId,
          blePlugin._lockServiceUUIDs,
          blePlugin._lockCharsUUIDs,
          (error, responseCharacteristic) => {
            count++;
            const data = Buffer.from(responseCharacteristic.value, 'base64');
            console.log(`Response Buffer: ${data.toString('hex')}`);
            arr.push(data);
            if (count === 11) {
              monitor.remove();
              resolve();
            }
          });
      });
      // const form = new FormData();
      // arr.forEach(buf => form.append('chunk', buf, { header: { 'Content-Length': buf.length }}));

      // const i = Buffer.from([33,22,11]);

      const boundary = Math.random().toString(36).slice(-10);
      const ending = `--${boundary}--\r\n`;
      const data = arr.map(buf => `--${boundary}\r\nContent-Length: ${buf.length}\r\n\r\n${buf}\r\n`).reduce((acc, cur) => acc + cur) + ending;
      const res = await axios.post('https://chunks.memfault.com/api/v0/chunks/TESTSERIAL', data , {
        headers: {
          'Content-Type': `multipart/mixed; boundary=${boundary}`,
          'Memfault-Project-Key': '9a9845167f5644d6a3fcf3386da97331',
          'Content-Length': data.length
        }
      })
      console.log(res.data);
      this.setState({ showActivity: false });
      alert(JSON.stringify(res));
    } catch (error) {
      this.setState({ showActivity: false });
      alert(error.message);
    }
  }

  setLockTime() {
    if (this.state.lock) {
      this.setState({ showActivity: true });
      this.state.lock.setLockTime().then((response) => {
        this.setState({ showActivity: false })
        alert(response.message)
      }).catch(error => {
        this.setState({ showActivity: false })
        alert(error.message);
      })
    }
    else {
      alert(initializeError)
    }
  }

  setDayLightSaving() {
    if (this.state.lock) {
      this.setState({ showActivity: true });
      this.state.lock.setDayLightSaving(this.state.dst).then((response) => {
        this.setState({ showActivity: false })
        alert(response.message)
      }).catch(error => {
        this.setState({ showActivity: false })
        alert(error.message);
      })
    }
    else {
      alert(initializeError)
    }
  }

  setLockTimeDST() {
    if (this.state.lock) {
      this.setState({ showActivity: true });
      this.state.lock.setLockTime(true).then((response) => {
        this.setState({ showActivity: false })
        alert(response.message)
      }).catch(error => {
        this.setState({ showActivity: false })
        alert(error.message);
      })
    }
    else {
      alert(initializeError)
    }
  }

  addCustomCode() {
    if (this.state.lock) {
      this.setState({ dialogVisible: true });
    }
    else {
      alert(initializeError)
    }
  }

  addCodeAction() {
    this.setState({ dialogVisible: false, showActivity: true });
    var startAt = new Date()
    var endAt = new Date()
    var endDate = new Date(endAt.setDate(endAt.getDate() + 15));

    this.state.lock.addPeriodPasscode(startAt.getTime(), endDate.getTime(), this.state.customCode, this.state.codeName).then(async (response) => {
      this.setState({ showActivity: false })
      if (response.success) {
          await createPeriodCode(
            this.state.lock.lockMac,
            startAt.getTime(),
            endDate.getTime(),
            this.state.customCode,
            this.state.codeName,
            this.state.lock.deviceAuthToken,
          );
        Alert.alert("Success!", "Code added successfully", [
          {
            text: "OK", onPress: () => {
              this.setState({ dialogVisible: false })
            }
          }
        ])
      } else {
        setTimeout(() => Alert.alert('Error', response.message), 500);
      }
    }).catch((err) => {
      this.setState({ showActivity: false });
      alertIOS(err.message);
    })
  }

  cancelAction() {
    this.setState({ dialogVisible: false, cyclicPopup: false, cyclicDaysPopup: false, isSetPassageMode: false })
  }

  displaySetResetButtonDialog = () => (
    <Overlay
      visible={this.state.setResetButtonDialogVisible}
      containerStyle={{ backgroundColor: 'rgba(0, 0, 0, 0.78)' }}
      childrenWrapperStyle={{ backgroundColor: '#eee' }}
      closeOnTouchOutside={false}
      animationDuration={500}>
      <View>
      <View style={styles.dialog}>
        <Text style={styles.dialogTextStyle}>
          Reset Button Enabled
        </Text>
        <Switch
          style={styles.switch}
          onValueChange={isResetButtonEnabled => this.setState({ isResetButtonEnabled })}
          value={this.state.isResetButtonEnabled}/>
      </View>
        <View style={styles.sliderView}>
          <View style={styles.dialog}>
            <TouchableHighlight style={styles.cancelButton} onPress={() => this.setState({ setResetButtonDialogVisible: false })}>
              <Text style={styles.buttonText}>
                Cancel
              </Text>
            </TouchableHighlight>
            <TouchableHighlight style={styles.submitButton} onPress={() => this.submitResetButtonEnabledAction()}>
              <Text style={styles.buttonText}>
                Submit
              </Text>
            </TouchableHighlight>
          </View>
        </View>
      </View>
    </Overlay>
  );

  displayDoorSensorDialog(isVisible) {
    if (isVisible) {
      return (
        <Overlay visible={isVisible}
          containerStyle={{ backgroundColor: 'rgba(0, 0, 0, 0.78)' }}
          childrenWrapperStyle={{ backgroundColor: '#eee' }}
          closeOnTouchOutside={false}
          animationDuration={500}>
          <View>
            <View style={styles.dialog}>
              <Text style={styles.dialogTextStyle}>
                Door Sensor Enabled
             </Text>
              <Switch style={styles.switch}
                onValueChange={(value) => this.setState({ isSensorEnabled: value })}
                disabled = {!this.state.isDoorSensorSupport}
                value={this.state.isSensorEnabled} />
            </View>
            <View style={styles.setLockView}>
              <Text style={styles.dialogTextStyle}>
                Support Door Sensor: {this.state.isDoorSensorSupport ? 'YES' : 'NO'}
              </Text>
            </View>
            <View style={styles.sliderView}>
              <View style={styles.dialog}>
                <TouchableHighlight style={styles.cancelButton} onPress={() => this.setState({ doorSensorDialogVisible: false })}>
                  <Text style={styles.buttonText}>
                    Cancel
                </Text>
                </TouchableHighlight>
                <TouchableHighlight style={styles.submitButton} onPress={() => this.submitDoorSensorEnabledAction()}>
                  <Text style={styles.buttonText}>
                    Submit
                </Text>
                </TouchableHighlight>
              </View>
            </View>
          </View>
        </Overlay>
      )
    }
    return null
  }

  displaySetAutoLockDialog(isVisible) {
    if (isVisible) {
      return (
        <Overlay visible={isVisible}
                 containerStyle={{ backgroundColor: 'rgba(0, 0, 0, 0.78)' }}
                 childrenWrapperStyle={{ backgroundColor: '#eee' }}
                 closeOnTouchOutside={false}
                 animationDuration={500}>
          <View>
            <View style={styles.dialog}>
              <Text style={styles.dialogTextStyle}>
                Auto Lock Enabled
              </Text>
              <Switch style={styles.switch}
                onValueChange={(value) => this.setState({ isAutoLockEnabled: value })}
                value={this.state.isAutoLockEnabled} />
            </View>
            <View style={styles.setLockView}>
              <Text style={styles.dialogTextStyle}>
                Set Auto Lock Time
              </Text>
            </View>
            <View style={styles.sliderView}>
              <Slider style={styles.sliderStyle}
                step={1}
                disabled={!this.state.isAutoLockEnabled}
                minimumValue={0}
                maximumValue={this.state.autoLockTimes.length - 1}
                onValueChange={val => this.setState({ autoLockInfo: { title: this.state.autoLockTimes[val].title, value: this.state.autoLockTimes[val].value } })}
                value={this.state.autoLockTimes.findIndex((el) => {
                  return el.value == this.state.autoLockInfo.value
                })}
              />
              <View style={styles.dialog}>
                <TouchableHighlight style={styles.cancelButton} onPress={() => this.setState({ autoLockDialogVisisble: false })}>
                  <Text style={styles.buttonText}>
                    Cancel
                  </Text>
                </TouchableHighlight>
                <TouchableHighlight style={styles.submitButton} onPress={() => this.submitAutoLockTimeAction()}>
                  <Text style={styles.buttonText}>
                    Submit
                  </Text>
                </TouchableHighlight>
              </View>
              <Text style={styles.sliderValue}>
                {this.state.autoLockInfo.title}
              </Text>
            </View>
          </View>
        </Overlay>
      )
    }
    return null
  }

  displayAddCodeDialog(isVisible) {
    if (isVisible) {
      return (
        <Dialog.Container visible={isVisible}>
          <Dialog.Title>ADD PERIOD CODE</Dialog.Title>
          <Dialog.Input label="CODE NAME:"
                        placeholder="type code name"
                        returnKeyType="done"
                        onChangeText={(text) => this.setState({ codeName: text })}
          >
          </Dialog.Input>
          <Dialog.Input label="PERIOD CODE:"
                        placeholder="type your code"
                        keyboardType="number-pad"
                        returnKeyType="done"
                        onChangeText={(text) => this.setState({ customCode: text })}
          >
          </Dialog.Input>
          {/* <Dialog.Input label="START DATE:">
                        </Dialog.Input>
                        <Dialog.Input label="END DATE:">
                        </Dialog.Input> */}
          <Dialog.Button label="Cancel"
                         onPress={() => this.cancelAction()} />
          <Dialog.Button label="Add Code"
                         onPress={() => this.addCodeAction()}
          />
        </Dialog.Container>
      )
    }
    return null
  }

  displayCyclicCodeDialog() {
    return (
      <Dialog.Container visible={this.state.cyclicPopup} contentStyle={{ marginBottom: 50 }}>
        <Dialog.Title>{this.state.isSetPassageMode ? "SET PASSAGE MODE" : ((this.state.isAddFOB) ? "ADD CYCLIC FOB" : "ADD CYCLIC CODE")}</Dialog.Title>
        {this.state.isSetPassageMode ?
          <View style={{ display: 'flex', flexDirection: 'row', justifyContent: 'center', alignItems: 'center', paddingBottom: 10 }}>
            <Text style={{ padding: 10 }}>Enable: </Text>
            <Switch
              onValueChange={isPassageModeEnabled => this.setState({ isPassageModeEnabled })}
              value={this.state.isPassageModeEnabled}
            />
          </View> :
          <Dialog.Input
            label={(this.state.isAddFOB) ? "FOB NAME: (optional)" : "CODE NAME: (optional)"}
            placeholder={(this.state.isAddFOB) ? "type fob name" : "type code name"}
            value={this.state.codeName} returnKeyType="done"
            onChangeText={(text) => this.setState({ codeName: text })}
          />}
        {(!this.state.isAddFOB && !this.state.isSetPassageMode) ?
        <Dialog.Input label="CYCLIC CODE:" placeholder="type your code"
        value={this.state.customCode} keyboardType= "number-pad" returnKeyType="done"
        onChangeText={(text) => this.setState({customCode: text})} /> : null
        }
         {(this.state.isAddFOB && (this.state.isAddCPUFOBWithNo || this.state.isAddFOBWithNo)) ?
        <Dialog.Input label="FOB NUMBER:" placeholder="type your number"
        value={this.state.customCode} keyboardType= "number-pad" returnKeyType="done" maxLength={10}
        onChangeText={(text) => this.setState({customCode: text})} /> : null
        }
        <Dialog.Input
          label="START DATE:"
          placeholder="YYYY-MM-DD"
          value={this.state.codeStartDate}
          keyboardType="number-pad"
          returnKeyType="done"
          maxLength={10}
          onChangeText={text => {
            if ((text.length === 5 && text.slice(-1) !== '-') || (text.length === 8 && text.slice(-1) !== '-')) {
              this.setState({ codeStartDate: text.substring(0, text.length - 1) + '-' + text.slice(-1) })
            } else {
              this.setState({ codeStartDate: text })
            }
          }}/>

        <Dialog.Input
          label="END DATE:"
          placeholder="YYYY-MM-DD"
          value={this.state.codeEndDate}
          keyboardType="number-pad"
          returnKeyType="done"
          maxLength={10}
          onChangeText={text => {
            if ((text.length === 5 && text.slice(-1) !== '-') || (text.length === 8 && text.slice(-1) !== '-')) {
              this.setState({ codeEndDate: text.substring(0, text.length - 1) + '-' + text.slice(-1) })
            } else {
              this.setState({ codeEndDate: text })
            }
          }}/>

        <Dialog.Input
          label="START TIME:" placeholder="HH:MM(:SS) eg. 13:00"
          value={this.state.codeStartTime}
          keyboardType="number-pad"
          returnKeyType="done"
          maxLength={8}
          onChangeText={text => {
            if ((text.length === 3 && text.slice(-1) !== ':') || (text.length === 6 && text.slice(-1) !== ':')) {
              this.setState({ codeStartTime: text.substring(0, text.length - 1) + ':' + text.slice(-1) })
            } else {
              this.setState({ codeStartTime: text })
            }
          }}/>

        <Dialog.Input
          label="END TIME:" placeholder="HH:MM(:SS) eg. 19:00"
          value={this.state.codeEndTime}
          keyboardType="number-pad"
          returnKeyType="done"
          maxLength={8}
          onChangeText={text => {
            if ((text.length === 3 && text.slice(-1) !== ':') || (text.length === 6 && text.slice(-1) !== ':')) {
              this.setState({ codeEndTime: text.substring(0, text.length - 1) + ':' + text.slice(-1) })
            } else {
              this.setState({ codeEndTime: text })
            }
          }}/>

        <Dialog.Button label="Cancel" onPress={() => this.cancelAction()}/>
        <Dialog.Button
          label={(this.state.isAddFOB && this.state.lock.lockType == lockType.V2LOCK) ? "Add FOB" : "Next"}
          onPress={() => {
            const [reg0, reg1, reg2, reg3] = [/\w{10}/, /\d\d\d\d-\d\d-\d\d/, /\d\d:\d\d/, /\d\d:\d\d:\d\d/];
            if (
              (this.state.isAddFOBWithNo || this.state.isAddCPUFOBWithNo) &&
              (this.state.customCode.length < 10 ||
                !reg0.test(this.state.customCode))
            ) {
              return alert('Fob ID should be 10 digit');
            }
            if (this.state.codeStartDate.length !== 0 && !(reg1.test(this.state.codeStartDate) && this.state.codeStartDate.length === 10)) {
              return alert('Start date format should follow YYYY-MM-DD')
            }
            if (this.state.codeEndDate.length !== 0 && !(reg1.test(this.state.codeEndDate) && this.state.codeEndDate.length === 10)) {
              return alert('End date format should follow YYYY-MM-DD')
            }
            if (reg2.test(this.state.codeStartTime) && this.state.codeStartTime.length === 5) {
              this.setState({ codeStartTime: this.state.codeStartTime + ':00' });
            } else if (!reg3.test(this.state.codeStartTime)) {
              return alert('Start time format should follow HH:MM(:SS)')
            }
            if (reg2.test(this.state.codeEndTime) && this.state.codeEndTime.length === 5) {
              this.setState({ codeEndTime: this.state.codeEndTime + ':00' });
            } else if (!reg3.test(this.state.codeEndTime)) {
              return alert('End time format should follow HH:MM(:SS)')
            }
            const startTimeRes = this.verifyTime(this.state.codeStartTime);
            const endTimeRes = this.verifyTime(this.state.codeEndTime);
            if (!startTimeRes) {
              return alert('Start time are out of range');
            }if (!endTimeRes) {
              return alert('End time are out of range');
            }
            const startDateRes = this.verifyDate(this.state.codeStartDate);
            const endDateRes = this.verifyDate(this.state.codeEndDate);
            if (!startDateRes) {
              return alert('Start date are out of range');
            }if (!endDateRes) {
              return alert('End date are out of range');
            }
            this.showCycleDaysPopup()
          }}
        />
      </Dialog.Container>
    )
  }

  verifyDate(date) {
    const y = parseInt(date.slice(0, 4));
    const m = parseInt(date.slice(5, 7));
    const d = parseInt(date.slice(8, 10));
    if (!(y >= 2000 && y < 2064 && m > 0 && m <= 12 && d > 0 && d <= 31)) {
      return false
    } else {
      return true;
    }
  }

  verifyTime(time) {
    const h = parseInt(time.slice(0, 2));
    const m = parseInt(time.slice(3, 5));
    const s = parseInt(time.slice(6, 8));
    if (!(h >= 0 && h < 24 && m >= 0 && m < 60 )) {
      return false
    } else if (s && !(s >= 0 && s < 60)) {
      return false
    } else {
      return true;
    }
  }

  showCycleDaysPopup() {
    if (this.state.isAddFOB && this.state.lock.lockType == lockType.V2LOCK) {
      if (this.state.isAddFOB && this.state.isAddCPUFOBWithNo) {
        this.addCPUICWithNumber();
      } else if (this.state.isAddFOB && this.state.isAddFOBWithNo) {
        this.addICWithNumber();
      } else if (this.state.isAddFOB) {
        this.addICCard();
      }
    }
    else {
      this.setState({ cyclicPopup: false, cyclicDaysPopup: true })
    }
    /*if (this.state.customCode.length === 0 || this.state.codeStartTime.length === 0 || this.state.codeEndTime.length === 0 ) {
      alert("Fill up required fields");
    } else {*/
    //}
  }

  displayCyclicDaysDialog() {
    return (
      <Dialog.Container visible={this.state.cyclicDaysPopup}>

        <Dialog.Title>SELECT CYCLIC DAYS</Dialog.Title>

        <Dialog.Switch label="Sunday" value={this.state.cycleDays[0]} onValueChange={() => this.dayChosen(0)} />
        <Dialog.Switch label="Monday" value={this.state.cycleDays[1]} onValueChange={() => this.dayChosen(1)} />
        <Dialog.Switch label="Tuesday" value={this.state.cycleDays[2]} onValueChange={() => this.dayChosen(2)} />
        <Dialog.Switch label="Wednesday" value={this.state.cycleDays[3]} onValueChange={() => this.dayChosen(3)} />
        <Dialog.Switch label="Thursday" value={this.state.cycleDays[4]} onValueChange={() => this.dayChosen(4)} />
        <Dialog.Switch label="Firday" value={this.state.cycleDays[5]} onValueChange={() => this.dayChosen(5)} />
        <Dialog.Switch label="Saturday" value={this.state.cycleDays[6]} onValueChange={() => this.dayChosen(6)} />

        <Dialog.Button label="Prev" onPress={() => this.cancelDaysAction()}/>
        <Dialog.Button label= {(this.state.isAddFOB) ? "Add FOB" : (this.state.isSetPassageMode) ? "Set Mode" : "Add Code"}
        onPress={() => {
            if (this.state.isSetPassageMode) {
              this.setPassageMode();
            } else if (this.state.isAddFOB && this.state.isAddCPUFOBWithNo) {
              this.addCPUICWithNumber();
            } else if(this.state.isAddFOB && this.state.isAddFOBWithNo) {
              this.addICWithNumber();
            }
            else if (this.state.isAddFOB) {
              this.addICCard()
            }
            else {
              this.addCyclicPasscode()
            }
          }
          }/>
      </Dialog.Container>
    )
  }

  cancelDaysAction() {
    this.setState({ cyclicPopup: true, cyclicDaysPopup: false })
  }

  dayChosen(day) {
    const { cycleDays } = this.state;
    if (cycleDays[day]) {
      cycleDays[day] = false
    } else {
      cycleDays[day] = true
    }
    this.setState({ cycleDays });
  }

  addCyclicPasscode() {
    this.setState({ showActivity:true, cyclicPopup: false, cyclicDaysPopup: false });
    this.state.lock.addCyclicPasscode(this.state.codeStartDate, this.state.codeEndDate, this.state.codeStartTime, this.state.codeEndTime,
      this.state.customCode, this.state.codeName, this.state.cycleDays).then(async (response)=>{
        if (response.success){
          const cycleType = convertCycleTypeToBinary(this.state.cycleDays);
          await createCyclicPasscode(
            this.state.lock.lockMac,
            this.state.codeStartDate,
            this.state.codeEndDate,
            this.state.codeStartTime.slice(0, -3),
            this.state.codeEndTime.slice(0, -3),
              this.state.customCode, this.state.codeName, cycleType, this.state.lock.deviceAuthToken);
          setTimeout(()=>{
            this.setState({showActivity:false});
            alert(`Code: ${response.code} with id: ${response.codeId} created successfully`);
          },300);
        } else {
          setTimeout(()=>{
            this.setState({showActivity:false});
            alert(response.message);
          },300);
        }
    }).catch((error)=>{
      setTimeout(()=>{
        this.setState({showActivity:false});
        alert(error.message)
      },300);
    })
  }

  addICCard() {
    this.setState({ showActivity:true, cyclicPopup: false, cyclicDaysPopup: false , isAddCPUFOBWithNo : false });
    this.state.lock
      .addICCard(
        this.state.codeStartDate,
        this.state.codeEndDate,
        this.state.codeStartTime,
        this.state.codeEndTime,
        this.state.codeName,
        this.state.cycleDays,
      )
      .then(async response => {
        if (response.success) {
          let cycleType = convertCycleTypeToBinary(this.state.cycleDays);
          if(cycleType === 0 ) cycleType = 128; // Period Fob if no days are selected
          await createFOB(
            this.state.lock.lockMac,
            this.state.codeName,
            response.fobNumber, // fob number is returned by the library
            this.state.codeStartDate,
            this.state.codeEndDate,
            this.state.codeStartTime.slice(0, -3),
            this.state.codeEndTime.slice(0, -3),
            cycleType,
            this.state.lock.deviceAuthToken,
          );
          setTimeout(()=>{
            this.setState({showActivity:false});
            alert(
              `Fob added with fobNumber: ${parseInt(response.fobNumber, 16)}`,
            );
          },300);
        } else {
          setTimeout(()=>{
            this.setState({showActivity:false});
            alert(response.message);
          },300
          );
        }
    }).catch((error)=>{
      setTimeout(()=>{
        this.setState({showActivity:false});
        alert(error.message)
      },300);
    })
  }



 addICWithNumber() {
    const fobId = parseInt(this.state.customCode, 10).toString(16).toUpperCase();
    this.setState({
      showActivity: true,
      cyclicPopup: false,
      cyclicDaysPopup: false,
      isAddFOBWithNo: false,
    });
    this.state.lock
      .addICCardWithNumber(
        this.state.codeStartDate,
        this.state.codeEndDate,
        this.state.codeStartTime,
        this.state.codeEndTime,
        fobId,
        this.state.codeName,
        this.state.cycleDays,
      )
      .then(async response => {
        if (response.success) {
          let cycleType = convertCycleTypeToBinary(this.state.cycleDays);
          if(cycleType === 0 ) cycleType = 128; // Period Fob if no days are selected
          await createFOB(
            this.state.lock.lockMac,
            this.state.codeName,
            fobId,
            this.state.codeStartDate,
            this.state.codeEndDate,
            this.state.codeStartTime.slice(0, -3),
            this.state.codeEndTime.slice(0, -3),
            cycleType,
            this.state.lock.deviceAuthToken,
          );
          setTimeout(() => {
            alert('Fob Added Successfully');
          }, 300);
        } else {
          setTimeout(() => {
            alert('Failed to add Fob');
          }, 300);
        }
      })
      .catch(error => {
        setTimeout(() => {
          alert(error.message);
        }, 300);
      })
      .finally(() => {
        this.setState({showActivity: false});
      });
  }

  addCPUICWithNumber() {
    const fobId = parseInt(this.state.customCode,10).toString(16);
    this.setState({ showActivity:true, cyclicPopup: false, cyclicDaysPopup: false , isAddCPUFOBWithNo : false });
    this.state.lock
      .addCPUCardWithNumber(
        this.state.codeStartDate,
        this.state.codeEndDate,
        this.state.codeStartTime,
        this.state.codeEndTime,
        fobId,
        this.state.codeName,
        this.state.cycleDays,
      )
      .then(async response => {
        if (response.success) {
          let cycleType = convertCycleTypeToBinary(this.state.cycleDays);
          if(cycleType === 0 ) cycleType = 128; // Period Fob if no days are selected
          await createFOB(
            this.state.lock.lockMac,
            this.state.codeName,
            fobId,
            this.state.codeStartDate,
            this.state.codeEndDate,
            this.state.codeStartTime.slice(0, -3),
            this.state.codeEndTime.slice(0, -3),
            cycleType,
            this.state.lock.deviceAuthToken,
          );
          setTimeout(()=>{
            this.setState({showActivity:false});
            alert(response.message);
          },300);
        } else {
          setTimeout(()=>{
            this.setState({showActivity:false});
            alert(response.message);
          },300);
        }
    }).catch((error)=>{
      setTimeout(()=>{
        this.setState({showActivity:false});
        alert(error.message)
      },300);
    })
  }

  async setPassageMode() {
    try {
      this.setState({ showActivity: true, cyclicPopup: false, cyclicDaysPopup: false, isAddFOBWithNo: false });
      const { success, message } = await this.state.lock.setPassageMode(this.state.isPassageModeEnabled, this.state.codeStartDate, this.state.codeEndDate, this.state.codeStartTime, this.state.codeEndTime, this.state.cycleDays);
      if (success) {
        alert('Set passage mode to ' + this.state.isPassageModeEnabled + ' succeed.');
      } else {
        alert(message);
      }
    } catch (err) {
      alert(err.message);
    } finally {
      this.setState({ showActivity: false });
    }
  }

  async getLog() {
    try {
      this.setState({ showActivity: true });
      const { success, message } = await this.state.lock.getLog();
      console.log('logs', message);
      this.setState({ showActivity: false });
      alert(message);
    } catch (error) {
      setTimeout(() => {
        this.setState({ showActivity: false });
        alert(error.message);
      }, 300);
    }
  }

  viewOperationLogs() {
    if (this.state.lock) {
      this.props.navigation.navigate('LogScreen', {
        lock: this.state.lock,
        onGoBack: this.onGoBack
      });
    }
    else {
      alert(initializeError)
    }
  }

  async checkStoragePermission() {
    const storagePermission = await PermissionsAndroid.check(
      PermissionsAndroid.PERMISSIONS.WRITE_EXTERNAL_STORAGE,
    );
    let granted;
    if (!storagePermission) {
      granted = await PermissionsAndroid.request(
        PermissionsAndroid.PERMISSIONS.WRITE_EXTERNAL_STORAGE,
      );
      if (granted !== PermissionsAndroid.RESULTS.GRANTED) {
        return false;
      }
    }
    return true;
  }

  upgradeFirmwareToLock = async (signedUrl, checksum, required_version) => {
    const downloads = RNFetchBlob.fs.dirs.DocumentDir;
    const res = await RNFetchBlob.config({
      fileCache: true,
      appendExt: 'zip',
      path: downloads + '/' + 'firmware' + Date.now() + '.zip',
    }).fetch('GET', signedUrl);

    console.log('The file saved to ', res.path());
    const calculatedFileChecksum = await RNFS.hash(res.path(), 'md5');
    if (calculatedFileChecksum !== checksum) {
      this.setState({
        DFUInProgress: false,
      });
      alertIOS('File could not be downloaded properly');
      return;
    }
    const destination = RNFS.CachesDirectoryPath + 'firmwareFile.zip';
    if (Platform.OS === 'ios' && await RNFS.exists(destination)) {
      await RNFS.unlink(destination);
    }
    await RNFS.copyFile(res.path(), destination);
    DFUEmitter.addListener(
      'DFUProgress',
      ({ percent }) => {
        if (percent == 100) {
          DFUEmitter.removeAllListeners('DFUStateChanged');
          DFUEmitter.addListener('DFUStateChanged', ({ state }) => {
            console.log('DFU_STATE', state);
            const states = this.state.DFUStates2;
            states.push(state);
            this.setState({ DFUStates2: states });
          });
        }
        this.setState({ DFUProgressPercent: percent });
      });

    DFUEmitter.addListener('DFUStateChanged', ({state}) => {
      console.log('DFU_STATE', state);
      const states = this.state.DFUStates1;
      states.push(state);
      this.setState({ DFUStates1: states });
    });
    this.setState({ DFUProgressBar: true });
    try {
      if ((this.state.lock.modelNum === 4 && this.state.lock.firmwareVer >= 16) || (this.state.lock.modelNum === 2 && this.state.lock.hardwareVer >= 3)) {
        await this.state.lock.enableFirmwareUpgrade();
      }
      const dfu = await NordicDFU.startDFU({
        deviceAddress: this.state.lock.deviceUUID,
        deviceName: this.state.lock.name,
        filePath: (Platform.OS === 'ios' ? 'file:///' : '') + destination,
      });
      this.props.navigation.goBack();
      alertIOS(`Firmware Upgrade Successful. Version: ${required_version}`);
    } catch (err) {
      if (err.message === 'DFU DEVICE DISCONNECTED') {
        alertIOS('Device Disconnected. Please make sure the device is in unlocked state and try again.');
      } else {
        alertIOS(err.message);
      }
    } finally {
      DFUEmitter.removeAllListeners('DFUProgress');
      DFUEmitter.removeAllListeners('DFUStateChanged');
      this.setState({
        DFUInProgress: false,
        DFUProgressBar: false,
        DFUProgressPercent: '0',
        DFUStates1: [],
        DFUStates2: [],
      });
    }
  };

  initiateFirmwareUpgrade = async () => {
    if (
      parseInt(this.state.firmwareVersion, 10) ===
      parseInt(this.state.lock.firmwareVer, 10)) {
      alertIOS('Lock already on the required firmware version.');
      return;
    }
    if (Platform.OS === 'android' && await this.checkStoragePermission() === false) {
      alertIOS('Need storage access for firmware upgrade');
      return;
    }
    this.setState({
      DFUInProgress: true,
    });
    let firmwareProperties;
    try {
      firmwareProperties = await getFirmwareUrl(
        this.state.lock.lockMac,
        this.state.lock.hardwareVer,
        this.state.firmwareVersion,
      );
    } catch (error) {
      this.setState({
        DFUInProgress: false,
        firmwareVersion: '',
      });
      if (error.message === 'no such firmware uploaded')
        alertIOS('No such firmware available');
      else alertIOS(error.message);
      return;
    }
    const {signedUrl, checksum, required_version} = firmwareProperties;
    if (
      parseInt(required_version.split('.')[1], 10) ===
      parseInt(this.state.lock.firmwareVer, 10)) {
      this.setState({
        DFUInProgress: false,
      });
      alertIOS('Lock already on the required firmware version.');
      return;
    }
    this.upgradeFirmwareToLock(signedUrl, checksum, required_version);
  };

  displayDFUDialog = () => {
    return (
      <Dialog.Container visible={this.state.DFUDialogVisible}>
        <Dialog.Title>Enter the firmware version</Dialog.Title>
        <Dialog.Description>
          Note: Submit as blank to get the version specified by the server.
        </Dialog.Description>
        <Dialog.Input
          label="Firmware Version:"
          placeholder="Eg: 9"
          value={this.state.firmwareVersion}
          returnKeyType="done"
          maxLength={2}
          onChangeText={firmwareVersion => this.setState({firmwareVersion})}
        />
        <Dialog.Button
          label="Cancel"
          onPress={() =>
            this.setState({
              firmwareVersion: '',
              DFUDialogVisible: false,
            })
          }
        />
        <Dialog.Button
          label="Submit"
          onPress={() => {
            this.setState({
              DFUDialogVisible: false,
            });
            this.initiateFirmwareUpgrade();
          }}
        />
      </Dialog.Container>
    );
  };

  render() {
    return (
      <View style={styles.container}>
        <View style={{ flexDirection: 'row' }}>
          <Text style={styles.title}>
            {'MAC : ' + this.state.lock.lockMac
              + '\nModel Num: ' + this.state.lock.modelNum
              + '    HW Ver: ' + this.state.lock.hardwareVer
              + '    FW Ver: ' + this.state.lock.firmwareVer + '\n'}
            {this.state.settingMode ||
              (this.state.masterCode ? ('Master Code : ' + this.state.masterCode + '') : 'Master Code : ')
              + (this.state.icCardSupported === undefined ? '\nSupport IC Card : ' : ('\nSupport IC Card : ' + (this.state.icCardSupported === true ? 'YES' : 'NO')))}
          </Text>
        </View>
        <ScrollView showsVerticalScrollIndicator={false}>
          <View style={styles.container}>
            {this.state.settingMode &&
            <TouchableHighlight style={styles.button} onPress={() => this.initLock()}>
              <Text style={styles.buttonText}>
                Init Lock
              </Text>
            </TouchableHighlight> ||
            <View>
              {this.state.lock.modelNum === 3 && <TouchableHighlight style={styles.button} onPress={() => this.props.navigation.navigate('NBLockScreen', { onGoBack: this.onGoBack, lock: this.state.lock })}>
                <Text style={styles.buttonText}>
                  5G functionalities
                </Text>
              </TouchableHighlight>}
              <TouchableHighlight style={styles.button} onPress={() => this.triggerMemfault()}>
                <Text style={styles.buttonText}>
                  Memfault Forward
                </Text>
              </TouchableHighlight>
              <View style={{ display: 'flex', flexDirection: 'row' }}>
                <TouchableHighlight style={[styles.button, { width: '40%' }]} onPress={() => this.setDayLightSaving()}>
                  <Text style={styles.buttonText}>
                    set DST
                  </Text>
                </TouchableHighlight>
                <CheckBox
                    containerStyle={styles.dstCheckbox}
                    title='Enable'
                    onPress={() => this.setState({ dst: !this.state.dst })}
                    checked={this.state.dst}
                />
              </View>
              <TouchableHighlight style={styles.button} onPress={() => this.setLockTime()}>
                <Text style={styles.buttonText}>
                  Set Clock
                </Text>
              </TouchableHighlight>
              <TouchableHighlight style={styles.button} onPress={() => this.readLockTime()}>
                <Text style={styles.buttonText}>
                  Read Clock
                </Text>
              </TouchableHighlight>
              <TouchableHighlight style={styles.button} onPress={() => this.unlockDevice()}>
                <Text style={styles.buttonText}>
                  Unlock
                </Text>
              </TouchableHighlight>
              <TouchableHighlight style={styles.button} onPress={() => this.lockDevice()}>
                <Text style={styles.buttonText}>
                  Lock
                </Text>
              </TouchableHighlight>
              <TouchableHighlight style={styles.button} onPress={() => this.testInstallationState()}>
                <Text style={styles.buttonText}>
                  Test Installation State
                </Text>
              </TouchableHighlight>
              <TouchableHighlight style={[styles.button, { backgroundColor: '#dd0000' }]} onPress={() => this.resetLock()}>
                <Text style={styles.buttonText}>
                  Reset Lock
                </Text>
              </TouchableHighlight>
              <TouchableHighlight style={styles.button} onPress={() => this.readAutoLockTime(true)}>
                <Text style={styles.buttonText}>
                  Read Auto Lock Time
                </Text>
              </TouchableHighlight>
              <TouchableHighlight style={styles.button} onPress={() => this.setAutoLockTime()}>
                <Text style={styles.buttonText}>
                  Set Auto Lock Time
                </Text>
              </TouchableHighlight>
              <TouchableHighlight style={styles.button} onPress={() => this.getBatteryInfo()}>
                <Text style={styles.buttonText}>
                  Get Battery
                </Text>
              </TouchableHighlight>
              <TouchableHighlight style={styles.button} onPress={() => this.addCustomCode()}>
                <Text style={styles.buttonText}>
                  Add Period Code
                </Text>
              </TouchableHighlight>
              {this.state.lock.lockType === lockType.V3LOCK &&
              <TouchableHighlight style={styles.button} onPress={() => this.createCyclicCode()}>
                <Text style={styles.buttonText}>
                  Add Cyclic Code
                </Text>
            </TouchableHighlight>
              }
              <TouchableHighlight style={styles.button} onPress={() => this.deletePasscodeAction(false)}>
                <Text style={styles.buttonText}>
                  Code List
                </Text>
              </TouchableHighlight>
              <TouchableHighlight style={styles.button} onPress={() => this.setResetButton()}>
                <Text style={styles.buttonText}>
                  Set Reset Button
                </Text>
              </TouchableHighlight>
              <TouchableHighlight style={styles.button} onPress={() => this.doorSensorAction()}>
                  <Text style={styles.buttonText}>
                    Door Sensor
                </Text>
                </TouchableHighlight>
                <TouchableHighlight style={styles.button} onPress={() => this.addCard()}>
                  <Text style={styles.buttonText}>
                    Add Fob
                </Text>
              </TouchableHighlight>
              <TouchableHighlight style={styles.button} onPress={() => this.addCardWithNo()}>
                <Text style={styles.buttonText}>
                  Add Fob by ID
                </Text>
              </TouchableHighlight>
              <TouchableHighlight style={styles.button} onPress={() => this.addCPUCardWithNo()}>
                  <Text style={styles.buttonText}>
                    Add CPU Fob by ID
                </Text>
              </TouchableHighlight>
              <TouchableHighlight style={styles.button} onPress={() => this.deletePasscodeAction(true)}>
                  <Text style={styles.buttonText}>
                    Fob List
                </Text>
              </TouchableHighlight>
              <TouchableHighlight style={styles.button} onPress={() => this.displayPassageModeDialog()}>
                <Text style={styles.buttonText}>
                  Set passage mode
                </Text>
              </TouchableHighlight>
              <TouchableHighlight style={styles.button} onPress={() => this.getLog()}>
                <Text style={styles.buttonText}>
                  Get Logs
                </Text>
              </TouchableHighlight>
              <TouchableHighlight style={styles.button} onPress={() => this.viewOperationLogs()}>
                <Text style={styles.buttonText}>
                  View Logs
                </Text>
              </TouchableHighlight>
                <TouchableHighlight
                  style={styles.button}
                  onPress={() =>
                    this.setState({
                      DFUDialogVisible: true,
                    })
                  }>
                  <Text style={styles.buttonText}>Upgrade Firmware</Text>
                </TouchableHighlight>
              <View style={{ height: 50 }}/>
              </View>
            }
            {this.displayAddCodeDialog(this.state.dialogVisible)}
            {this.displayDoorSensorDialog(this.state.doorSensorDialogVisible)}
            {this.displaySetAutoLockDialog(this.state.autoLockDialogVisisble)}
            {this.state.cyclicPopup &&
            this.displayCyclicCodeDialog()
            }
            {this.state.cyclicDaysPopup &&
            this.displayCyclicDaysDialog()
            }
            {this.state.DFUDialogVisible && this.displayDFUDialog()}
            {this.displayInstallationReport()}
            {this.displaySetResetButtonDialog()}
          </View>
        </ScrollView>
        <Modal
          animationType="fade"
          transparent={true}
          visible={this.state.DFUProgressBar}>
          <View style={{
            backgroundColor: 'white',
            opacity: 0.8,
            height: Dimensions.get('window').height,
            width: Dimensions.get('window').width,
            display: 'flex',
            justifyContent: 'center',
            alignItems: 'center',
          }}>
            <View style={{ flex: 6, justifyContent: 'flex-end' }}>
              {this.state.DFUStates1.map(state => <Text>{state}</Text>)}
            </View>
            <View
              style={{
                flex: 1,
                alignItems: 'center',
                justifyContent: 'center',
              }}>
              <ProgressBar
                progress={this.state.DFUProgressPercent / 100}
                height={16}
                width={160}
              />
              <Text style={styles.spinnerTextStyle}> {`${this.state.DFUProgressPercent}%`}</Text>
              {this.state.DFUProgressPercent > 0 &&
              <Text style={styles.spinnerTextStyle}>{'Firmware Upgrade in Progress. Please wait...'}</Text>}
            </View>
            <View style={{ flex: 6 }}>
              {this.state.DFUStates2.map(state => <Text>{state}</Text>)}
            </View>
          </View>
        </Modal>
        <LoadingIndicator showActivity={this.state.showActivity} />
        {!this.state.DFUProgressBar && (<Spinner
            visible={this.state.DFUInProgress}
            textContent={'Initiating Firmware Upgrade. Please wait...'}
            textStyle={styles.spinnerTextStyle}
            customIndicator={
              <LoadingIndicator showActivity={this.state.DFUInProgress} />
            }
          />
        )}
      </View>
    );
  }
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    alignItems: 'center'
  },
  title: {
    alignItems: 'center',
    justifyContent: 'center',
    textAlign: 'center',
    margin: 5,
    fontSize: 16,
    fontWeight: 'bold',
    color: buttonColor
  },
  buttonText: {
    alignItems: 'center',
    justifyContent: 'center',
    textAlign: 'center',
    paddingLeft: 10,
    paddingRight: 10,
    fontSize: 16,
    color: '#ffffff',
    fontWeight: 'bold'
  },
  dstCheckbox: {
    marginTop: 35,
  },
  cancelButton: {
    marginRight: 30,
    alignItems: 'center',
    justifyContent: 'center',
    height: 60,
    width: 110,
    marginTop: 30,
    backgroundColor: '#1166bb',
    borderRadius: 5,
  },
  submitButton: {
    alignItems: 'center',
    justifyContent: 'center',
    height: 60,
    width: 110,
    marginTop: 30,
    backgroundColor: '#1166bb',
    borderRadius: 5,
  },
  button: {
    alignItems: 'center',
    justifyContent: 'center',
    width: 250,
    height: 60,
    marginTop: 30,
    backgroundColor: buttonColor,
    borderRadius: 5,
  },
  dialog: {
    flexDirection: 'row',
    alignItems: 'center',
  },

  switch: {
    right: 0,
    top: -5,
    position: 'absolute'
  },
  sliderValue: {
    right: 0,
    position: 'absolute',
    fontSize: 20,
    fontWeight: 'bold'
  },
  setLockView: {
    top: 20,
    flexDirection: 'row',
    alignItems: 'center',
  },
  sliderView:
    {
      top: 5,
      alignItems: 'center'
    },
  sliderStyle: {
    top: 20,
    width: 300,
    alignItems: 'center'
  },
  dialogTextStyle: {
    fontSize: 17
  },
  spinnerTextStyle: {
    color: 'black',
    fontSize: 15,
  },
});

export default LockOperationScreen;
