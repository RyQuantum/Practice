import sqlite3
import logging
from flask import Flask, jsonify, request, g, json

logging.basicConfig(level=logging.DEBUG,
                format='%(asctime)s %(filename)s[line:%(lineno)d] %(levelname)s %(message)s',
                datefmt='%a, %d %b %Y %H:%M:%S',
                filename='app.log',
                filemode='w')

console = logging.StreamHandler()
console.setLevel(logging.INFO)
formatter = logging.Formatter('%(name)-12s: %(levelname)-8s %(message)s')
console.setFormatter(formatter)
logging.getLogger('').addHandler(console)

app = Flask(__name__)
app.config['JSON_AS_ASCII'] = False

conn = sqlite3.connect('sqlite.db')
try:
    conn.execute('create table if not exists locks(id integer primary key autoincrement, serial_number varchar(20))')
except:
    logging.warning('Create table failed')
finally:
    conn.commit()
    conn.close()


def connect_db():
    return sqlite3.connect('sqlite.db')


@app.before_request
def before_request():
    g.db = connect_db()


@app.teardown_request
def teardown_request(exception):
    if hasattr(g, 'db'):
        g.db.close()


@app.route('/lock', methods=['POST'])
def upload_serial_number():
    # print(request.get_json()['serial_number'])
    # print(request.form['serial_number'])
    # print('stream ' + str(request.stream.read()))
    # return jsonify({'success': True}), 200, {'ContentType': 'application/json'}
    # print('form', request.form)
    serial_number = request.get_json()['serial_number']
    logging.info('serial_number: ' + serial_number)
    # print('insert into locks (serial_number) values("' + serial_number + '")')
    g.db.execute('insert into locks (serial_number) values("' + serial_number + '")')
    g.db.commit()
    # for lock in query_db('select * from locks'):
    #     print(lock['serial_number'], 'has the id', lock['id'])
    # print('serial_number', urllib.parse.parse_qs(request.get_data().decode("utf-8")))
    return jsonify({'success': True, 'message': serial_number + ' saved'}), 200, {
        'ContentType': 'application/json'}


@app.route('/')
def hello_world():
    return 'Hello World'


def query_db(query, args=(), one=False):
    cur = g.db.execute(query, args)
    rv = [dict((cur.description[idx][0], value)
               for idx, value in enumerate(row)) for row in cur.fetchall()]
    return (rv[0] if rv else None) if one else rv


if __name__ == "__main__":
    app.run(host='0.0.0.0', port=25348)
