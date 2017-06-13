from struct import unpack, calcsize
from sys import argv
from os import makedirs
from os.path import isdir

if len(argv) < 2:
	print("%s file.bin" % (argv[0]))
	exit(1)
f = argv[1]

header = 'IIf'
header_size = calcsize(header)
typ8 = 'III'
typ8_size = calcsize(typ8)
typ5 = 'IHI'
typ5_size = calcsize(typ8)

with open(f, 'rb') as file:
	
	file.seek(0, 2)
	end = file.tell()
	file.seek(0, 0)
	
	if not isdir(f + '.packets'): makedirs(f + '.packets')
	
	while end > file.tell():
	
		(size, type, time) = unpack(header, file.read(header_size))
		
		here = file.tell()

		typeSTR = hex(type)

		filename = '%f.packet' % (time)

		if type == 0x8 or type == 0x7:
			typeSTR = typeSTR + '/' + hex(unpack(typ8, file.read(typ8_size))[1])
		if type == 0x5:
			typeSTR = typeSTR + '/' + hex(unpack(typ5, file.read(typ5_size))[1])
			
		file.seek(here, 0)
		
		dirname = f + '.packets/' + typeSTR
		
		if not isdir(dirname): makedirs(dirname)
		
		with open(dirname + '/' + filename, 'wb') as packet:
			packet.write(file.read(size))
		