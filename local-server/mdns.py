import argparse
import logging
import socket
from zeroconf import Zeroconf, IPVersion, ServiceInfo


def broadcast():
    logging.basicConfig(level=logging.DEBUG)

    parser = argparse.ArgumentParser()
    parser.add_argument('--debug', action='store_true')
    version_group = parser.add_mutually_exclusive_group()
    version_group.add_argument('--v6', action='store_true')
    version_group.add_argument('--v6-only', action='store_true')
    args = parser.parse_args()

    if args.debug:
        logging.getLogger('zeroconf').setLevel(logging.DEBUG)
    if args.v6:
        ip_version = IPVersion.All
    elif args.v6_only:
        ip_version = IPVersion.V6Only
    else:
        ip_version = IPVersion.V4Only

    desc = {'path': '/~ryan/'}

    s = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
    s.connect(("8.8.8.8", 80))
    ip = s.getsockname()[0]
    s.close()

    info = ServiceInfo(
        "_http._tcp.local.",
        "Ryan's MacBook Air._http._tcp.local.",
        addresses=[socket.inet_aton(ip)],
        port=80,
        properties=desc,
        server="ryan-MBA.local.",
    )

    zeroconf = Zeroconf(ip_version=ip_version)

    print("Registration of a service")
    zeroconf.register_service(info)
    