import React from 'react';
import {
  Alert,
  PermissionsAndroid,
  StyleSheet,
  Platform,
  Text,
  TouchableHighlight,
  View,
  ActivityIndicator,
  Image,
  Keyboard
} from 'react-native';
import { blueLogin } from '../services/APIManager';
import {userType, userID, backgroundColor, buttonColor, bleVer, MapKeylessToOss} from '../services/Constants';
import { BleManager } from 'react-native-ble-plx';
import { Dropdown } from 'react-native-material-dropdown';
import { TextField } from 'react-native-material-textfield';
import axios from 'axios';

const APIs = [
  { value: 'Production'},
  { value: 'Staging'},
  { value: 'Blue'},
  { value: 'Opensesame'},
  { value: 'Certify'},
  { value: 'Red'},
  { value: 'Black'},
  { value: 'Green'},
  { value: 'QE'},
];
if (__DEV__) {
  APIs.unshift({ value: '192.168.1.8:3000' });
}

const getPermission = async () => {
  if (Platform.OS === 'ios') {
    return true;
  } else {
    const granted = await PermissionsAndroid.check(PermissionsAndroid.PERMISSIONS.ACCESS_FINE_LOCATION);
    if (!granted) {
      try {
        const granted = await PermissionsAndroid.request(
          PermissionsAndroid.PERMISSIONS.ACCESS_COARSE_LOCATION,
          {
            'title': 'Demo App',
            'message': 'Demo app needs access to your location'
          }
        )
        if (granted === PermissionsAndroid.RESULTS.GRANTED) {
          return true;
        } else {
          return false;
        }
      } catch (err) {
      }
    }
  }
  return false;
}

class LoginScreen extends React.Component {

  static navigationOptions = {
    title: 'Login',
    headerBackTitle: null,
  };

  constructor(props) {
    super(props);
    this.state = {
      username: __DEV__ ? 'admin@gmail.com' : '',
      password: __DEV__ ? 'admin' : '',
      env: '',
      loading: false,
      expireDeviceToken: false, //?
      forbiddenDeviceToken: false, //?
      // isAPIEnabled: false, //?
      // guestDeviceToken: false, //?
      bleManager: new BleManager()
    };
  }

  componentDidMount() {
    axios.interceptors.request.use(req =>{
      console.log(req);
    }, err=>{});
  }

  login = async () => {
    const serializeJSON = function(data) {
      return Object.keys(data).map(function (keyName) {
        return encodeURIComponent(keyName) + '=' + encodeURIComponent(data[keyName])
      }).join('&');
    }

    const i = Buffer.from([44, 33, 22, 11, 0, 11, 22, 33]);

    // const F = require('./form_data');
    // const f = new F();
    // f.append('chunk', i, { header: { 'Content-Length': i.length } });
    const formData = new FormData();
    formData.append('name[x]', i);
    formData.append('y', {
      type: 'application/octet-stream',
      // uri: i,
      header: { type: 'application/json' },
      // name: 'y'
    }, 'z')
    formData.getParts();
    const boundary = Math.random().toString(36).slice(-10);
    const sss = `--${boundary}\r\nContent-Length: ${i.length}\r\n\r\n${i}\r\n`;
    // const ending = `--${boundary}--\r\n`;

    // const res = await fetch('http://192.168.2.247/api/v0/chunks/TESTSERIAL', {
    //   method: 'POST',
    //   headers: {
    //     'Content-Type': `multipart/mixed; boundary=${boundary}`,
    //     'Memfault-Project-Key': '9a9845167f5644d6a3fcf3386da97331',
    //   },
    //   body: sss + sss + sss + ending
    // });

    const buf1 = Buffer.from([33, 22, 11, 0, 11, 22, 33]);
    const buf2 = Buffer.from([33, 22, 11, 0, 11, 22, 33]);
    const arr = [buf1, buf2]
    // const boundary = Math.random().toString(36).slice(-10);
    const ending = `--${boundary}--\r\n`;
    const data = arr.map(buf => `--${boundary}\r\nContent-Length: ${buf.length}\r\n\r\n${buf.toString()}\r\n`).reduce((acc, cur) => acc + cur) + ending;
    console.log('data', data, data.length)
    // const res = await axios.post('http://192.168.2.247/api/v0/chunks/TESTSERIAL', data , {
    //   headers: {
    //     'Content-Type': `multipart/mixed; boundary=${boundary}`,
    //     'Memfault-Project-Key': '9a9845167f5644d6a3fcf3386da97331',
    //     'Content-Length': data.length
    //   }
    // })
    // const res = await fetch('http://192.168.2.247/api/v0/chunks/TESTSERIAL', {
    //     method: 'POST',
    //     headers: {
    //       'Content-Type': `multipart/mixed; boundary=${boundary}`,
    //       'Memfault-Project-Key': '9a9845167f5644d6a3fcf3386da97331',
    //     },
    //     body: f
    //   });
    const res = await axios.post('http://192.168.2.247/api/v0/chunks/TESTSERIAL',formData, {
      headers: {
        'Content-Type': `multipart/mixed; boundary=${boundary}`,
        'Memfault-Project-Key': '9a9845167f5644d6a3fcf3386da97331',
      }
    })
    console.log(res.data);
  };

  onGoBack = () => {
    this.setState({
      username: '',
      password: '',
    });
  };

  expireValueChange = (value) => {
    if (value && this.state.forbiddenDeviceToken) {
      this.setState({ expireDeviceToken: value, forbiddenDeviceToken: !value });
    } else {
      this.setState({ expireDeviceToken: value });
    }
  };

  submit = async () => {
    if (await getPermission()) {
      if (await this.state.bleManager.state() !== 'PoweredOff') {
        if (!this.state.loading) {
          Keyboard.dismiss();
          this.login();
        }
      } else {
        Alert.alert('Info', `Please turn on bluetooth and retry login.`);
      }
    }
  };

  render() {
    return (
      <View style={styles.container}>
        <Image source={require('../assets/logo.png')} style={styles.logo} />
        <View style={{ width: '80%' }}>
        <View style={{ flexDirection: 'row'}}>
          <Dropdown
            label='Environment'
            value={this.state.env ? MapKeylessToOss.find(item => item.keyless === this.state.env).name : this.state.env}
            dropdownMargins={{ min: 8, max: 16 }}
            containerStyle={{ width: '66%' }}
            pickerStyle={{ height: 340 }}
            data={APIs}
            onChangeText={environment => {
                let urlsObj = MapKeylessToOss.find(
                  item => item.name === environment,
                );
                let env = urlsObj.keyless;
                this.setState({env});
            }}
          />
          <Text style={{ paddingTop: 35, paddingLeft: 30, }}>BLE lib: {bleVer}</Text>
        </View>

          <TextField
            label={'Username: '}
            value={this.state.username}
            onChangeText={username => this.setState({ username })}
            autoCorrect={false}
            autoFocus={true}
            autoCapitalize='none'
            keyboardType='email-address'
          />

          <TextField
            label={'Password: '}
            value={this.state.password}
            onChangeText={password => this.setState({ password })}
            onSubmitEditing={this.submit}
            returnKeyType="go"
            autoCapilatize='none'
            autoCorrect={false}
            secureTextEntry
          />
        </View>

          <TouchableHighlight
            style={styles.button}
            underlayColor='transparent'
            enabled={!this.state.loading}
            onPress={this.submit}>
            <View style={styles.buttonInnerView}>
              <Text style={styles.buttonText}>Integration Partner Login</Text>
            </View>
          </TouchableHighlight>
        {this.state.loading &&
          <View style={{ height: 50, alignItems: 'center' }}>
            <ActivityIndicator />
          </View> ||
          <View style={{ height: 50 }} />
        }
      </View>
    );
  }
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: backgroundColor,
    alignItems: 'center',
    justifyContent: 'flex-start'
  },
  logo: {
    resizeMode: 'contain',
    height: 140,
    width: '60%',
    alignSelf: 'center'
  },
  button: {
    width: 200,
    height: 50,
    margin: 40
  },
  buttonInnerView: {
    flex: 1,
    justifyContent: 'center',
    margin: 5,
    borderRadius: 20,
    backgroundColor: buttonColor,
  },
  buttonText: { height: 20, alignSelf: 'center', color: 'white', fontSize: 14 },
  switchView: { flexDirection: 'row', alignItems: 'stretch', justifyContent: 'space-between', margin: 20 },
  switchContainer: { flexDirection: 'column', alignItems: 'stretch', justifyContent: 'center' }
});

export default LoginScreen;
