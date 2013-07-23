# -*- coding: utf-8 -*-
import ConfigParser
import os.path


def is_append_mode(projectPath, section_name, option_name):
    config_path = projectPath + '/Classes/build_xcode.ini'
    #section_name = 'FB_SETTING_STATE'
    #option_name = 'is_fb_set'

    if os.path.isfile(config_path):
        config = ConfigParser.ConfigParser()
        config.read(config_path)

        if config.has_option(section_name, option_name):
            value = config.get(section_name, option_name, 0)
            print('value : ' + value)

            if value == 'True':
                return True
            else:
                return False
        else:
            create_new_config(config_path, section_name, option_name, 'ab')
            return False
    else:
        create_new_config(config_path, section_name, option_name, 'wb')
        return False


def create_new_config(config_path, section_name, option_name, file_option):
    config = ConfigParser.RawConfigParser()
    config.add_section(section_name)
    config.set(section_name, option_name, 'True')

    with open(config_path, file_option) as configfile:
        config.write(configfile)
