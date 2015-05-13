import sys
#  sys는 Python의 기본 라이브러리로써 에러 메시지 출력과 프로그램 강제 종료 기능을 제공한다.
def func(operator):
    # 함수가 호출되면 countline을 1 증가시킨 후 각각의 명령어에 따라 하위함수를 호출하는 함수이다.
    # 하위함수에 할당되지 않는 명령어가 전달되면 에러메시지를 출력하고 프로그램을 종료시킨다.
    global countline
    countline += 1
    if(operator=='notop' or operator=='neg' or operator=='inc' or operator=='dec' or operator=='dup'):
        singlecalculate(operator)
    elif(operator=='add' or operator=='sub' or operator=='mult' or operator=='div' or operator=='mod' or operator=='swp' or operator=='and' or operator=='or'):
        doublecalculate(operator)
    elif(operator=='gt' or operator=='lt' or operator=='ge' or operator=='le' or operator=='eq' or operator=='ne'):
        compare(operator)
    elif(operator=='lod' or operator=='str' or operator=='ldc' or operator=='lda'):
        stackOperator(operator)
    elif(operator=='ujp' or operator=='tjp' or operator=='fjp'):
        jump(operator)
    elif(operator=='chkh' or operator=='chkl'):
        rangeChecking(operator)
    elif(operator=='ldi' or operator=='sti'):
        indirectAddressing(operator)
    elif(operator=='call' or operator=='ret' or operator=='retv' or operator=='ldp' or operator=='proc'):
        functionCR(operator)
    elif(operator=='sym' or operator=='nop' or operator=='bgn' or operator=='end'):
        pseudoInst(operator)
    else:
        sys.stderr.write('Command Error!!!')
        sys.exit()

def singlecalculate(operator):
     #단일 항 연산을 하는 명령어들을 수행하는 함수이다.
    a=stack.pop()
      # stack[top]을 pop한 후
    if(operator=='notop'):
         # not연산을 하고 stack에 push한다.
        stack.append(~a)
    elif(operator=='neg'):
         # 해당 값의 부호를 바꿔준 후 stack에 push한다.
        stack.append(0-a)
    elif(operator=='inc'):
          # 해당 값을 1증가시킨 후 stack에 push한다.
        stack.append(a+1)
    elif(operator=='dec'):
          # 해당 값을 1감소시킨 후 stack에 push한다.
        stack.append(a-1)
    elif(operator=='dup'):
          #  해당 값을 stack에 두 번 push한다.
        stack.append(a)
        stack.append(a)
def doublecalculate(operator):
      # 이항 연산을 하는 명령어들을 수행하는 함수이다.
    b=stack.pop()
    a=stack.pop()
    # stack[top-1]과 stack[top]을 pop한 후
    if(operator=='add'):
        # 해당 값을 더하여 stack에 push한다.
        stack.append(a+b)
    elif(operator=='sub'):
        # 해당 값을 빼 stack에 push한다.
        stack.append(a-b)
    elif(operator=='mult'):
        # 해당 값을 곱하여 stack에 push한다.
        stack.append(a*b)
    elif(operator=='div'):
        # 해당 값을 나누어 stack에 push한다
        stack.append(a/b)
    elif(operator=='mod'):
        # 해당 값을 나머지연산을 하여 stack에 push한다.
        stack.append(a%b)
    elif(operator=='swp'):
        # 해당 값의 위치를 바꾸어 stack에 push한다.
        stack.append(b)
        stack.append(a)
    elif(operator=='and'):
         # 해당 값을 AND연산하여 stack에 push한다.
        stack.append(a&b)
    elif(operator=='or'):
         # 해당 값을 OR연산하여 stack에 push한다.
        stack.append(a|b)
def compare(operator):
    # stack의 두 값을 비교하는 연산을 하는 명령어들을 수행하는 함수이다.
    b=stack.pop()
    a=stack.pop()
    # stack[top-1]과 stack[top]을 pop한 후 
    if(operator=='gt'):
        # 두 값을 비교하여 앞의 값이 더 클 경우 True를 아니면 False를 stack에 push한다.
        stack.append(a>b)
    elif(operator=='lt'):
        # 두 값을 비교하여 앞의 값이 더 작을 경우 True를 아니면 False를 stack에 push한다.
        stack.append(a<b)
    elif(operator=='ge'):
        # 두 값을 비교하여 앞의 값이 크거나 같을 경우 True를 아니면 False를 stack에 push한다.
        stack.append(a>=b)
    elif(operator=='le'):
        # 두 값을 비교하여 앞의 값이 작거나 같을 경우 True를 아니면 False를 stack에 push한다.
        stack.append(a<=b)
    elif(operator=='eq'):
        # 두 값을 비교하여 앞의 값이 같을 경우 True를 아니면 False를 stack에 push한다.
        stack.append(a==b)
    elif(operator=='ne'):
        # 두 값을 비교하여 앞의 값이 다를 경우 True를 아니면 False를 stack에 push한다.
        stack.append(a!=b)
def stackOperator(operator):
    #stack에 값을 불러오거나 stack에서 memory로 값을 저장하는 명령어들을 수행한다.
    global loadFlag, memory, loadStack, stack, currentBlock
    if(operator=='lod'):
        # memory로부터 해당 위치의 값을 찾아 stack에 push한다.
        if loadFlag == 1:
            # loadFlag가 1일 경우 stack이 아니라 loadStack에 push한다.
            if list[2] == '1':
                loadStack.append(memory[int(list[2])][int(list[3])])
            else:
                loadStack.append(memory[currentBlock][int(list[3])])
        elif list[2] == '1':
            stack.append(memory[int(list[2])][int(list[3])])
        else:
            stack.append(memory[currentBlock][int(list[3])])
    elif(operator=='str'):
       # stack[top]의 값을 pop하여 memory의 해당 위치에 저장한다.
        if list[2] == '1':
            memory[int(list[2])][int(list[3])] = stack.pop()
        else:
            memory[currentBlock][int(list[3])] = stack.pop()
    elif(operator=='ldc'):
        # 상수를 stack에 push한다.
        if loadFlag == 1:
         # loadFlag가 1일 경우 stack이 아니라 loadStack에 push한다.
            loadStack.append(int(list[2]))
        else:
            stack.append(int(list[2]))
    elif(operator=='lda'):
        # memory로부터 해당 위치의 값을 찾아 그 값의 주소를 stack에 push한다.
        if loadFlag == 1:
            # loadFlag가 1일 경우 stack이 아니라 loadStack에 push한다.
            if list[2] == '1':
                loadStack.append(int(list[2])*10000+int(list[3]))
            else:
                loadStack.append(currentBlock*10000+int(list[3]))
        elif list[2] == '1':
            stack.append(int(list[2])*10000+int(list[3]))
        else:
            stack.append(currentBlock*10000+int(list[3]))
def jump(operator):
    # ujp, tjp, fjp와 같이 특정 위치로 점프하는 명령어들을 수행한다.
    global countline, label
    if(operator=='ujp'):
         # 무조건 해당 분기로 점프한다.
        countline = label[list[2]]
    elif(operator=='tjp'):
        # stack[top]의 값이 True일 경우 해당 분기로 점프한다.
        if stack.pop() == True:
            countline = label[list[2]]
    elif(operator=='fjp'):
        # stack[top]의 값이 False일 경우 해당 분기로 점프한다.
        if stack.pop() == False:
            countline = label[list[2]]
def rangeChecking(operator):
    # 에러를 체크하는 명령어들을 수행한다.
    if(operator=='chkh'):
        if(int(list[2])>=stack[-1]):
            # stack[top]의 값을 주어진 값보다 큰지 확인한다. 
            # 위의 결과가 False라면 에러를 발생시키고 프로그램을 종료시킨다.
            sys.stderr.write('CHK Error!!!')
            sys.exit()
    elif(operator=='chkl'):
            # stack[top]의 값을 주어진 값보다 작은지 확인한다. 
            # 위의 결과가 False라면 에러를 발생시키고 프로그램을 종료시킨다.
        if(int(list[2])<=stack[-1]):
            sys.stderr.write('CHK Error!!!')
            sys.exit()
def indirectAddressing(operator):
    #메모리의 특정 주소에 접근하는 명령어들을 수행한다.
    global loadFlag, loadStack, stack, memory
    if(operator=='ldi'):
        addr = stack.pop()
        if loadFlag == 1:
            loadStack.append(memory[addr/10000][addr%10000])
        else:
            stack.append(memory[addr/10000][addr%10000])
    elif(operator=='sti'):
        value = stack.pop()
        addr = stack.pop()
        memory[addr/10000][addr%10000] = value
def functionCR(operator):
    global stack, memory, countline, loadFlag, callStack, currentBlock
    if(operator=='call'):
          # read, write, lf와 같은 제공함수와 사용자 정의함수를 호출한다.
        if(list[2] == 'read'):
            addr = loadStack.pop()
            memory[addr/10000][addr%10000] = input()
            loadFlag = 0
        elif(list[2] == 'write'):
            sys.stdout.write(str(loadStack.pop()) + ' ')
            loadFlag = 0
        elif(list[2] == 'lf'):
            print
        else:
            callStack.append(countline)
            countline = label[list[2]]
    elif(operator=='ret'):
        # void를 return한다.
        pass
    elif(operator=='retv'):
        # stack[top]에 있는 값을 return한다.
        pass
    elif(operator=='ldp'):
        # loadFlag를 1로 만들어 이후에 stack에 push되는 값들은
        # loadstack에 push하여 이후에 호출되는 함수의 파라미터로 넘긴다.
        loadFlag = 1
    elif(operator=='proc'):
        # 새로운 함수를 선언하고 해당 함수의 시작을 나타낸다.
        currentBlock += 1
        memory.append([0])
def pseudoInst(operator):
     # 의사 지시 명령어들을 수행한다.
    global loadStack, memory, MainEndFlag, countline, callStack, loadFlag, currentBlock
    if(operator=='sym'):
          # 새로운 변수를 선언하여 메모리에 공간을 할당하거나, 함수 호출시 전달된 파라미터들을 메모리에 저장한다.
        if len(loadStack) != 0:
            memory[currentBlock].append(loadStack.pop(0))
            if len(loadStack) == 0:
                loadFlag = 0
        else:
            loadFlag = 0
            for i in range(int(list[4])):
                memory[currentBlock].append(0)

    elif(operator=='bgn' and MainEndFlag == 1):
         # 프로그램의 시작을 나타낸다. 전달되는 값만큼 전역변수가 있음을 나타낸다.
        currentBlock += 1
        MainEndFlag = 0
        memory.append([0])
        countline = countline - int(list[2]) - 1
    elif(operator=='end'):
        # 전체 프로그램 또는 각 함수의 끝을 나타낸다.
        memory.pop()
        currentBlock -= 1
        countline = callStack.pop()

###### MAIN FUNCTION START ######
name = raw_input()
if name.endswith(".uco"):   # 파일의 확장자가 .uco인지 확인한다.
    f = open(name, 'r')
    lines=f.readlines()    # .uco 파일의 모든 줄의 정보를 담고 있는 배열이다.
    stack=[]               # 프로그램 내부에서 stack역할을 위한 배열이다.
    memory=[0]             # .uco 파일의 변수들을 저장하기 위한 배열이다.
    currentBlock = 0       # 함수들의 메모리를 구분해주기 위한 변수이다.
    callStack = []  # 함수 호출 후 리턴시 돌아갈 줄 번호를 저장하기 위한 배열이다.
    loadStack = []  # 함수 호출시 변수들을 전달하기 위해서 사용한 특별한 배열이다
    label={}        # 각각의 레이블과 시작줄의 줄 번호를 저장하기 위한 dictionary이다.
    countline=0     # 다음에 실행할 줄의 번호를 저장하기 위한 변수이다.
    MainEndFlag=1   # 프로그램의 시작과 끝의 정보를 label에 넣기위해 임시로 사용한 변수이다.
    loadFlag = 0    # 함수호출시 loadStack을 사용하기 위해 임시로 사용한 변수이다.

    # l은 .uco 파일의 각각의 줄에 레이블이 있는지 여부를 확인하기 위해 임시로 사용한 변수이다.
     # list는 .uco 파일의 한 줄을 공백기준으로 나누어 명령수행을 쉽게 하기위해 임시로 선언한 변수이다.
    for l in lines:
        #l에 lines복사한다.
        lines[countline] = ' '.join(lines[countline].split())
         #lines에 있는 공백을 없앤다.
        if l[0] == ' ':
             #l의 첫 번째 칸이 공백이면 (라벨이 없으면) lines앞에 공백 추가한다.
            lines[countline] = ' '+lines[countline]
             #list에 lines의 공백 기준으로 나누어 대입
        list=lines[countline].split(' ')
        if list[0]!='':
              # list[0]이 공백이 아니면 label에 label이름과 그 label 이름이 위치한 줄 대입한다.
            label[list[0]]=countline
        if list[1]=='bgn':
            #list[1]이 bgn이면 label에 label이름과 그 label이름이 위치한 줄 대입
            label[list[1]]=countline
            MainEndFlag=0
        elif list[1]=='end' and MainEndFlag==0:
            label[list[1]]=countline
            MainEndFlag=1
        countline += 1

    countline = label.get('bgn')
    while 1:
        # raw_input()                                     # for decode
        # print('code : ' + str(lines[countline]))        # for decode
        list=lines[countline].split(' ')                # 
        # print('next line number : ' + str(countline + 1))                            # for decode
        func(list[1])                                               # do command
        # print('loadFlag : ' + str(loadFlag))            # for decode
        # print('loadStack : ' + str(loadStack))          # for decode
        # print('currentBlock : ' + str(currentBlock))    # for decode
        # print('memory : ' + str(memory))                # for decode
        # print('stack : ' + str(stack))                  # for decode
        if countline == label.get('end'):
            # #label[]에서 ‘end'가 저장되어 있는 배열의 값이 countline과 일치한다면, while문을 빠져나온다.
            break
    f.close()
     #file을 닫는다.
else:
    print('file format is wrong!!!')
