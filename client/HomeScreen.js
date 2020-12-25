import React from 'react';
import { FlatList, StyleSheet, Text, TouchableHighlight, View, ActivityIndicator} from "react-native";
import { getDeviceToken, getLockDetails } from "../services/APIManager";
import DeviceListItem from '../components/DeviceListItem';
import {userType, userID, backgroundColor, buttonColor, button2Color, MapKeylessToOss} from '../services/Constants';
import { OaksBleLockLibrary, RNBlePlugin, PersistencePlugin } from '../BleLibrary/lib';
import Dialog from 'react-native-dialog';
import LoadingIndicator from '../components/LoadingIndicator';
export let objRNOaksOpenMobileLib;

class RefreshBtn extends React.Component {

  constructor(props) {
    super(props);
    this.state = {
      loading: true
    };
    this.props.navigation.setParams({ clickRefreshBtn: this.clickRefreshBtn });
    this.timerId = setTimeout(() => {
      this.setState({ loading: false });
    }, 5100);
  }

  clickRefreshBtn = () => {
    this.setState({loading: true});
    clearTimeout(this.timerId);
    this.timerId = setTimeout(() => {
      this.setState({ loading: false });
    }, 5100);
  }

  render() {
    return this.state.loading ? <ActivityIndicator size="large"/> : (
        <TouchableHighlight
            underlayColor='transparent'
            onPress={async () => {
              this.clickRefreshBtn();
              await this.props.navigation.state.params.refreshKeys();
            }}>
          <View style={{
            flex: 1,
            justifyContent: 'center',
            margin: 5,
            width: 60,
            borderRadius: 30,
            backgroundColor: buttonColor,
          }}>
            <Text style={{height: 20, alignSelf: 'center', color: 'white', fontSize: 14}}>Refresh</Text>
          </View>
        </TouchableHighlight>
    )
  }
}

class HomeScreen extends React.Component {
  static navigationOptions = ({ navigation }) => ({
    title: 'Oaks Open Library Demo',
    headerRight: <RefreshBtn navigation={navigation}/>,
  });

  constructor(props) {
    super(props);
    this.state = {
      loading: false,
      locks: {},
      selectedItem: null,
      keys: {},
      userId: this.props.navigation.state.params.userId,
      host: this.props.navigation.state.params.host,
      expireDeviceToken: this.props.navigation.state.params.expireDeviceToken,
      forbiddenDeviceToken: this.props.navigation.state.params.forbiddenDeviceToken,
      isAPIEnabled: this.props.navigation.state.params.isAPIEnabled,
      role: this.props.navigation.state.params.role,
      isLockMacDialogVisible: false,
      NBlockMac: '',
      isNBLockMacLoading: false,
    };
    // instantiate OaksBleLockLibrary by passing getToken function and environment
    try {
      this.persistentPlugin = new PersistencePlugin();
      this.blePlugin = new RNBlePlugin();
      objRNOaksOpenMobileLib = new OaksBleLockLibrary(this.getToken, this.blePlugin, this.persistentPlugin);
      objRNOaksOpenMobileLib.userId = 123;
    } catch (error) {
      console.log({error})
      alert(error);
    }
    const baseUrl = this.props.navigation.state.params.host.hostUrl.split('/')[2];
    const urlObj = MapKeylessToOss.find(item => item.keyless === baseUrl);
    //set the respective openapi env url in the library
    urlObj
      ? OaksBleLockLibrary.setHostUrl(`https://${urlObj.openapi}/oakslock/`)
      : OaksBleLockLibrary.setHostUrl(
          this.props.navigation.state.params.host.hostUrl + 'oakslock/',
        );
  }

  UNSAFE_componentWillMount() {
    this.scanForDevices();
  }

  componentDidMount() {
    this.props.navigation.setParams({
      refreshKeys: this.refreshListOnClick,
    });
  }

  componentWillUnmount() {
    this.stopScanningDevices();
  }

  onGoBack = () => {
    this.setState({locks: {}});
    this.stopScanningDevices();
    this.scanForDevices();
    this.props.navigation.state.params.clickRefreshBtn();
  };

  getToken = async (lockMac, fromHome = false) => {

    // fromHome is true when app calls this method to create device instance
    // then creating instance with expired/forbidden token if expireDeviceToken/forbiddenDeviceToken is true for testing
    // library will get 401/403 for expired/forbidden token
    // library will call this method then fromHome will take default false value and this method will return new token by calling api

    try {
      return await getDeviceToken(lockMac, this.state.role);
    } catch (error) {
      return error;
    }
  };

  getDetails = async (lockMac, deviceToken) => {
    try {
      return await getLockDetails(lockMac, deviceToken);
    } catch (error) {
      return error;
    }
  };

  scanForDevices() {
    if (objRNOaksOpenMobileLib) {
      try {
        objRNOaksOpenMobileLib.on('foundDevice', this.foundLock);
        this.setState({tempLocks:{}});
        objRNOaksOpenMobileLib.startScan(5);
      } catch (err) {
        console.log('error ',err)
        alert(err);
      }
    }
  }

  stopScanningDevices() {
    if (objRNOaksOpenMobileLib) {
      try {
        objRNOaksOpenMobileLib.stopScan();
        objRNOaksOpenMobileLib.removeListener('foundDevice', this.foundLock);
      } catch (err) {
        alert(err);
      }
    }
  }

  refreshListOnClick = () => {
    this.setState({locks: {}},()=>{
      this.stopScanningDevices();
      this.scanForDevices();
    });
  };

  foundLock = async device => {
    console.log('Device in demo app: ' + device.lockMac);
    const { locks: lockList = {}} = this.state;
    const lockMac = device.lockMac;

    if (!lockList[lockMac]) {
      try {
        // const { accessToken, expiresAt } = await this.getToken(lockMac, true);
        const lock = objRNOaksOpenMobileLib.createDevice(device);
        // lock.refreshToken();
        this.setState({locks: {...this.state.locks, [lockMac]: lock}});
      } catch (error) {
        console.log(error);
      }
    } else {
      lockList[lockMac].settingMode = device.settingMode;
      lockList[lockMac].touch = device.touch;
      lockList[lockMac].battery = device.battery;
      lockList[lockMac].rssi = device.rssi;
      lockList[lockMac].firmwareVer = device.firmwareVer;
      lockList[lockMac].hardwareVer = device.hardwareVer;
      this.setState({ locks: { ...lockList }});
    }
  }

  FlatListItemSeparator = () => {
    return (
      <View
        style={{
          height: 1,
          width: '100%',
          backgroundColor: '#D3D3D3',
        }}
      />
    );
  }

  _renderItem = ({item}) => (
    <DeviceListItem
      {...this.props}
      onGoBack={this.onGoBack}
      item={item}
      onPressItem={this._onPressItem}
      onPressUnlock={this._onPressUnlock}
    >
    </DeviceListItem>
  );

  _onPressItem = (item) => {
    if (!item.deviceAuthToken) {
      alert('Retrieving device token, wait...');
      return;
    }
    item.isAPIEnabled = this.state.isAPIEnabled;
    this.props.navigation.navigate('Details', {item, blePlugin: this.blePlugin, onGoBack: this.onGoBack});
  };

  _onPressUnlock = (item) => {
    if (!item.deviceAuthToken) {
      alert('Retrieving device token, wait...');
      return;
    }
    if (!item.ekey && !item.settingMode) {
      alert('ekey not found for this lock.');
      return;
    }
    this.parallelOperationTestWithUnlockOperation(item);
  };

  parallelOperationTestWithUnlockOperation(item){
    item.unlock().then((resp)=>{
      alert(resp.message);
    }).catch((error) => {
      alert(error);
    })
  }

  _keyExtractor = (item) => item.lockMac;

  submitLockMac = async () => {
    const lockMacRegex = /^([0-9A-F]{2}[:]){5}([0-9A-F]{2})$/;
    if (!lockMacRegex.test(this.state.NBlockMac)) {
      alert('Invalid Lockmac');
      return;
    }
    this.setState({
      isNBLockMacLoading: true,
      isLockMacDialogVisible: false,
    });
    const getTokenObj = await this.getToken(this.state.NBlockMac);
    this.setState({isNBLockMacLoading: false});
    if (!getTokenObj.accessToken) {
      alert(getTokenObj); // error is returned if getToken fails.
      return;
    }
    const {accessToken} = getTokenObj;
    this.setState({isNBLockMacLoading: true});
    const lockDetails = await this.getDetails(
      this.state.NBlockMac,
      accessToken,
    );
    this.setState({isNBLockMacLoading: false});
    if (!lockDetails.lockMac) {
      alert(lockDetails.message); // error is returned if getDetails fail.
      return;
    }
    if (lockDetails.devicetype !== '5GLock') {
      alert('Not a 5G Lock. Enter Lockmac of 5GLock');
      return;
    }
    const lock = {
      lockMac: this.state.NBlockMac,
      deviceAuthToken: accessToken,
    };
    this.setState({
      NBlockMac: '',
    });
    this.props.navigation.navigate('NBLockScreen', {
      onGoBack: this.onGoBack,
      lock: lock,
      lockDetails: lockDetails.deviceDetails || null,
      previousScreen: this.props.navigation.state.routeName,
      title: '5G Lock Operations',
    });
  };

  enterLockMacDialog = () => {
    return (
      <Dialog.Container visible={this.state.isLockMacDialogVisible}>
        <Dialog.Title>Enter Lockmac For 5G Lock</Dialog.Title>
        <Dialog.Input
          placeholder="Eg: FD:F2:F3:F4:F5:F6"
          value={this.state.NBlockMac}
          returnKeyType="done"
          maxLength={17}
          onChangeText={NBlockMac => this.setState({NBlockMac})}
        />
        <Dialog.Button
          label="Cancel"
          onPress={() =>
            this.setState({
              NBlockMac: '',
              isLockMacDialogVisible: false,
            })
          }
        />
        <Dialog.Button label="Submit" onPress={this.submitLockMac} />
      </Dialog.Container>
    );
  };


  render() {
    return (
      <View style={styles.container}>
        <View>
          <Text style={styles.text}>{this.state.host.hostUrl}</Text>
        </View>
        <View style={styles.lockMacContainer}>
          <TouchableHighlight
            style={{width: 150, height:40, margin:5}}
            underlayColor='transparent'
            onPress={() =>  this.setState({
                isLockMacDialogVisible: true,
              })
            }>
            <View style={styles.buttonInnerView}>
              <Text style={styles.buttonText}>Enter 5G LockMac</Text>
            </View>
          </TouchableHighlight>
        </View>
        <View style={styles.flatView}>
          <FlatList data={Object.values(this.state.locks).sort((a, b) => {
            if (a.touch && !b.touch) return -1;
            else if (!a.touch && b.touch) return 1;
            else if (a.ekey && !b.ekey) return -1;
            else if (!a.ekey && b.ekey) return 1;
            else return a.lockMac - b.lockMac;
          })}
                    keyExtractor={this._keyExtractor}
                    ItemSeparatorComponent={this.FlatListItemSeparator}
                    extraData={this.state}
                    renderItem={this._renderItem}/>
        </View>
        {this.state.isLockMacDialogVisible && this.enterLockMacDialog()}
        <LoadingIndicator showActivity={this.state.isNBLockMacLoading}/>
      </View>
    );
  }
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: button2Color,
  },
  text: {
    color: 'white',
    alignSelf: 'center',
    borderWidth: 1,
    width: '101%',
    textAlign: 'center',
    borderColor: backgroundColor,
  },
  flatView: {
    flex: 1,
    backgroundColor: backgroundColor,
  },
  lockMacContainer: {
    height: '8%',
    backgroundColor: backgroundColor,
  },
  buttonInnerView: {
    flex: 1,
    justifyContent: 'center',
    margin: 5,
    borderRadius: 20,
    backgroundColor: buttonColor,
  },
  buttonText: {height: 20, alignSelf: 'center', color: 'white', fontSize: 14},
});

export default HomeScreen;
