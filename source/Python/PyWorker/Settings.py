import sys
import yaml

def get_settings(filename):
    f = open(filename, mode='rt', encoding='utf-8')
    data = yaml.load(f.read())
    f.close()
    return data

if __name__ == '__main__':
    get_settings(sys.argv[1])
