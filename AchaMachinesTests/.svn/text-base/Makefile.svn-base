
EXTRA_DIST =  Makefile

# Warning: This is an automatically generated file, do not edit!

srcdir=.
top_srcdir=..

include $(top_srcdir)/config.make

ifeq ($(CONFIG),DEBUG)
ASSEMBLY_COMPILER_COMMAND = gmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize+ -debug "-define:DEBUG"
ASSEMBLY = bin/Debug/AchaMachinesTests.exe
ASSEMBLY_MDB = $(ASSEMBLY).mdb
COMPILE_TARGET = exe
PROJECT_REFERENCES =  \
	../AchaMachines/bin/Debug/AchaMachines.dll \
	../AchaMachinesExamples/bin/Debug/AchaMachinesExamples.dll
BUILD_DIR = bin/Debug

ACHAMACHINES_DLL_MDB_SOURCE=../AchaMachines/bin/Debug/AchaMachines.dll.mdb
ACHAMACHINES_DLL_MDB=$(BUILD_DIR)/AchaMachines.dll.mdb
ACHAMACHINESTESTS_EXE_MDB_SOURCE=bin/Debug/AchaMachinesTests.exe.mdb
ACHAMACHINESTESTS_EXE_MDB=$(BUILD_DIR)/AchaMachinesTests.exe.mdb
ACHAMACHINES_DLL_SOURCE=../AchaMachines/bin/Debug/AchaMachines.dll
ACHAMACHINESEXAMPLES_DLL_MDB_SOURCE=../AchaMachinesExamples/bin/Debug/AchaMachinesExamples.dll.mdb
ACHAMACHINESEXAMPLES_DLL_MDB=$(BUILD_DIR)/AchaMachinesExamples.dll.mdb
ACHAMACHINESEXAMPLES_DLL_SOURCE=../AchaMachinesExamples/bin/Debug/AchaMachinesExamples.dll

endif

ifeq ($(CONFIG),RELEASE)
ASSEMBLY_COMPILER_COMMAND = gmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize+
ASSEMBLY = bin/Release/AchaMachinesTests.exe
ASSEMBLY_MDB = 
COMPILE_TARGET = exe
PROJECT_REFERENCES =  \
	../AchaMachines/bin/Release/AchaMachines.dll \
	../AchaMachinesExamples/bin/Release/AchaMachinesExamples.dll
BUILD_DIR = bin/Release

ACHAMACHINES_DLL_MDB=
ACHAMACHINESTESTS_EXE_MDB=
ACHAMACHINES_DLL_SOURCE=../AchaMachines/bin/Release/AchaMachines.dll
ACHAMACHINESEXAMPLES_DLL_MDB=
ACHAMACHINESEXAMPLES_DLL_SOURCE=../AchaMachinesExamples/bin/Release/AchaMachinesExamples.dll

endif

AL=al2
SATELLITE_ASSEMBLY_NAME=$(notdir $(basename $(ASSEMBLY))).resources.dll

PROGRAMFILES = \
	$(ACHAMACHINES_DLL_MDB) \
	$(ACHAMACHINESTESTS_EXE_MDB) \
	$(ACHAMACHINES_DLL) \
	$(ACHAMACHINESEXAMPLES_DLL_MDB) \
	$(ACHAMACHINESEXAMPLES_DLL)  

BINARIES = \
	$(ACHAMACHINESTESTS)  


RESGEN=resgen2

ACHAMACHINESTESTS = $(BUILD_DIR)/achamachinestests
ACHAMACHINES_DLL = $(BUILD_DIR)/AchaMachines.dll
ACHAMACHINESEXAMPLES_DLL = $(BUILD_DIR)/AchaMachinesExamples.dll

FILES = \
	AchaMachinesTests.cs 

DATA_FILES = 

RESOURCES = 

EXTRAS = \
	achamachinestests.in 

REFERENCES = 

DLL_REFERENCES = 

CLEANFILES = $(PROGRAMFILES) $(BINARIES) 

#Targets
all: $(ASSEMBLY) $(PROGRAMFILES) $(BINARIES)  $(top_srcdir)/config.make

include $(top_srcdir)/Makefile.include
#include $(srcdir)/custom-hooks.make



$(eval $(call emit-deploy-target,ACHAMACHINES_DLL_MDB))
$(eval $(call emit-deploy-wrapper,ACHAMACHINESTESTS,achamachinestests,x))
$(eval $(call emit-deploy-target,ACHAMACHINES_DLL))
$(eval $(call emit-deploy-target,ACHAMACHINESEXAMPLES_DLL_MDB))
$(eval $(call emit-deploy-target,ACHAMACHINESEXAMPLES_DLL))


$(eval $(call emit_resgen_targets))
$(build_xamlg_list): %.xaml.g.cs: %.xaml
	xamlg '$<'


$(ASSEMBLY) $(ASSEMBLY_MDB): $(build_sources) $(build_resources) $(build_datafiles) $(DLL_REFERENCES) $(PROJECT_REFERENCES) $(build_xamlg_list) $(build_satellite_assembly_list)
	make pre-all-local-hook prefix=$(prefix)
	mkdir -p $(shell dirname $(ASSEMBLY))
	make $(CONFIG)_BeforeBuild
	$(ASSEMBLY_COMPILER_COMMAND) $(ASSEMBLY_COMPILER_FLAGS) -out:$(ASSEMBLY) -target:$(COMPILE_TARGET) $(build_sources_embed) $(build_resources_embed) $(build_references_ref)
	make $(CONFIG)_AfterBuild
	make post-all-local-hook prefix=$(prefix)

install-local: $(ASSEMBLY) $(ASSEMBLY_MDB)
	make pre-install-local-hook prefix=$(prefix)
	make install-satellite-assemblies prefix=$(prefix)
	mkdir -p '$(DESTDIR)$(libdir)/$(PACKAGE)'
	$(call cp,$(ASSEMBLY),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call cp,$(ASSEMBLY_MDB),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call cp,$(ACHAMACHINES_DLL_MDB),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call cp,$(ACHAMACHINESTESTS_EXE_MDB),$(DESTDIR)$(libdir)/$(PACKAGE))
	mkdir -p '$(DESTDIR)$(bindir)'
	$(call cp,$(ACHAMACHINESTESTS),$(DESTDIR)$(bindir))
	$(call cp,$(ACHAMACHINES_DLL),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call cp,$(ACHAMACHINESEXAMPLES_DLL_MDB),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call cp,$(ACHAMACHINESEXAMPLES_DLL),$(DESTDIR)$(libdir)/$(PACKAGE))
	make post-install-local-hook prefix=$(prefix)

uninstall-local: $(ASSEMBLY) $(ASSEMBLY_MDB)
	make pre-uninstall-local-hook prefix=$(prefix)
	make uninstall-satellite-assemblies prefix=$(prefix)
	$(call rm,$(ASSEMBLY),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call rm,$(ASSEMBLY_MDB),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call rm,$(ACHAMACHINES_DLL_MDB),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call rm,$(ACHAMACHINESTESTS_EXE_MDB),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call rm,$(ACHAMACHINESTESTS),$(DESTDIR)$(bindir))
	$(call rm,$(ACHAMACHINES_DLL),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call rm,$(ACHAMACHINESEXAMPLES_DLL_MDB),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call rm,$(ACHAMACHINESEXAMPLES_DLL),$(DESTDIR)$(libdir)/$(PACKAGE))
	make post-uninstall-local-hook prefix=$(prefix)

install: install-local
uninstall: uninstall-local
clean: clean-local

include $(top_srcdir)/rules.make
