
EXTRA_DIST = Twofish.make Makefile

all: all-recursive

top_srcdir=..
include $(top_srcdir)/config.make
include $(top_srcdir)/Makefile.include
include $(top_srcdir)/rules.make

#include $(top_srcdir)/custom-hooks.make

#Warning: This is an automatically generated file, do not edit!
ifeq ($(CONFIG),DEBUG)
 SUBDIRS =  . 
endif
ifeq ($(CONFIG),RELEASE)
 SUBDIRS =  . 
endif

# Include project specific makefile
include Twofish.make

CONFIG_MAKE=$(top_srcdir)/config.make

%-recursive: $(CONFIG_MAKE)
	@set . $$MAKEFLAGS; final_exit=:; \
	case $$2 in --unix) shift ;; esac; \
	case $$2 in *=*) dk="exit 1" ;; *k*) dk=: ;; *) dk="exit 1" ;; esac; \
	make pre-$*-hook prefix=$(prefix) ; \
	for dir in $(call quote_each,$(SUBDIRS)); do \
		case "$$dir" in \
		.) make $*-local || { final_exit="exit 1"; $$dk; };;\
		*) (cd "$$dir" && make $*) || { final_exit="exit 1"; $$dk; };;\
		esac \
	done; \
	make post-$*-hook prefix=$(prefix) ; \
	$$final_exit

$(CONFIG_MAKE):
	echo "You must run configure first"
	exit 1

clean: clean-recursive
install: install-recursive
uninstall: uninstall-recursive

install: install-local
uninstall: uninstall-local
clean: clean-local

dist-local: dist-local-recursive
