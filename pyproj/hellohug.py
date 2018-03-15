import hug
@hug.get()
def hello():
    '''say hello'''
    return 'hello'